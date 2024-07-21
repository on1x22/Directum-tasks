using PersonalMeetingsManager.DAL.Models;
using PersonalMeetingsManager.DAL.Repository;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PersonalMeetingsManager.Domain.CommandHandlers
{
    internal class UpdateMeetingCommandHandler
    {
        private readonly IMeetingsRepository _repository;
        private readonly IConsoleCommandHandler _consoleCommandHandler;
        private readonly IFormatProvider provider = new CultureInfo("ru-RU");

        internal UpdateMeetingCommandHandler(IMeetingsRepository repository,
                                             IConsoleCommandHandler consoleCommandHandler)
        {
            _repository = repository;
            _consoleCommandHandler = consoleCommandHandler;
        }
        internal async Task UpdateMeetingAsync(string meetingIdString)
        {
            //IFormatProvider provider = new CultureInfo("ru-RU");
            if (!int.TryParse(meetingIdString, out int meetingId))
            {
                Console.WriteLine("Задан некорректный номер встречи.Операция изменения встречи отменена");
                return;
            }

            var meetingFromDb = await _repository.GetMeetingByIdAsync(meetingId);
            var meetingForUpdate = /*new Meeting() { Id = meetingFromDb.Id };*/meetingFromDb.Clone();
            if (meetingFromDb == null)
            {
                Console.WriteLine($"Встреча с Id={meetingId} не существует");
                return;
            }

            //var command = $"Старое значение: {meetingFromDb.Subject}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новую тему встречи: ";
            ///*Console.WriteLine($"Старое значение: {meetingFromDb.Subject}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новую тему встречи: ");*/
            //_consoleCommandHandler.WriteCommandToConsole(command);
            //var subject = Console.ReadLine();
            //if (!string.IsNullOrEmpty(subject))
            //    meetingForUpdate.Subject = subject;
            UpdateSubject(meetingFromDb, meetingForUpdate);

            //command = $"Старое значение: {meetingFromDb.StartDateTime}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новое время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            ///*Console.WriteLine($"Старое значение: {meetingFromDb.StartDateTime}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новое время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");*/
            //_consoleCommandHandler.WriteCommandToConsole(command);
            //var startDateTimeString = Console.ReadLine();
            //if (!string.IsNullOrEmpty(startDateTimeString))
            //{
            //    DateTime startDateTime;
            //    try
            //    {
            //        startDateTime = DateTime.Parse(startDateTimeString, provider);
            //    }
            //    catch (FormatException)
            //    {
            //        Console.WriteLine("Задана некорректная дата начала встречи. Операция изменения встречи отменена");
            //        return;
            //    }
            //    if (startDateTime <= DateTime.Now)
            //    {
            //        Console.WriteLine("Встреча может быть назначена только на будущее время. Операция изменения встречи отменена");
            //        return;
            //    }
            //    meetingForUpdate.StartDateTime = startDateTime;
            //}
            if (!TryUpdateStartDateTime(meetingFromDb, meetingForUpdate))
                return;

            //command = $"Старое значение: {meetingFromDb.EndDateTime}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новое время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            ///*Console.WriteLine($"Старое значение: {meetingFromDb.EndDateTime}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новое время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");*/
            //_consoleCommandHandler.WriteCommandToConsole(command);
            //var endDateTimeString = Console.ReadLine();
            //if (!string.IsNullOrEmpty(endDateTimeString))
            //{
            //    DateTime endDateTime;
            //    try
            //    {
            //        endDateTime = DateTime.Parse(endDateTimeString, provider);
            //    }
            //    catch (FormatException)
            //    {
            //        Console.WriteLine("Задана некорректная дата окончания встречи. Операция изменения встречи отменена");
            //        return;
            //    }
            //    if (endDateTime <= meetingForUpdate.StartDateTime)
            //    {
            //        Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция изменения встречи отменена");
            //        return;
            //    }
            //    meetingForUpdate.EndDateTime = endDateTime;
            //}
            if (!TryUpdateFinishDateTime(meetingFromDb, meetingForUpdate))
                return;

            //command = $"Старое значение:{meetingFromDb.MeetingNotificationTimeInMinutes}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новое время напоминания о встрече в минутах: ";
            ///*Console.WriteLine($"Старое значение:{meetingFromDb.MeetingNotificationTimeInMinutes}\n" +
            //    $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
            //    $"Введите новое время напоминания о встрече в минутах: ");*/
            //_consoleCommandHandler.WriteCommandToConsole(command);
            //var notificationTimeString = Console.ReadLine();
            //if (!string.IsNullOrEmpty(notificationTimeString))
            //{
            //    if (!int.TryParse(notificationTimeString, out int notificationTime))
            //    {
            //        Console.WriteLine("Задано некорректное время напоминания о встрече. Операция изменения встречи отменена");
            //        return;
            //    }

            //    meetingForUpdate.MeetingNotificationTimeInMinutes = notificationTime;
            //}
            if (!TryUpdateNotificationTimeInMinute(meetingFromDb, meetingForUpdate))
                return;

            var isMeetingDatesIntersect = await _repository.CheckIntersectMeetingDatesAsync(meetingForUpdate);
            if (isMeetingDatesIntersect)
            {
                Console.WriteLine("Время измененной встречи пересекается с уже сущестующей встречей. Операция создания новой встречи отменена");
                return;
            }
            if (!await CheckIntersectMeetingDatesAsync(meetingForUpdate))
                return;

            var updatedMeeting = await _repository.UpdateMeetingAsync(meetingForUpdate);
            Console.WriteLine("Встреча изменена:");
            Console.WriteLine(MeetingInfoFormatter.GetMeetingInfoString(updatedMeeting));
        }

        private void UpdateSubject(Meeting meetingFromDb, Meeting meetingForUpdate)
        {
            var command = $"Старое значение: {meetingFromDb.Subject}\n" +
                $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
                $"Введите новую тему встречи: ";
            
            _consoleCommandHandler.WriteCommandToConsole(command);
            var subject = Console.ReadLine();
            if (!string.IsNullOrEmpty(subject))
                meetingForUpdate.Subject = subject;
        }

        private bool TryUpdateStartDateTime(Meeting meetingFromDb, Meeting meetingForUpdate)
        {
            var command = $"Старое значение: {meetingFromDb.StartDateTime}\n" +
                $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
                $"Введите новое время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            
            _consoleCommandHandler.WriteCommandToConsole(command);
            var startDateTimeString = Console.ReadLine();
            if (!string.IsNullOrEmpty(startDateTimeString))
            {
                DateTime startDateTime;
                try
                {
                    startDateTime = DateTime.Parse(startDateTimeString, provider);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Задана некорректная дата начала встречи. Операция изменения встречи отменена");
                    return false;
                }
                if (startDateTime <= DateTime.Now)
                {
                    Console.WriteLine("Встреча может быть назначена только на будущее время. Операция изменения встречи отменена");
                    return false;
                }
                meetingForUpdate.StartDateTime = startDateTime;
            }
            return true;
        }

        private bool TryUpdateFinishDateTime(Meeting meetingFromDb, Meeting meetingForUpdate)
        {
            var command = $"Старое значение: {meetingFromDb.EndDateTime}\n" +
                $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
                $"Введите новое время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            
            _consoleCommandHandler.WriteCommandToConsole(command);
            var endDateTimeString = Console.ReadLine();
            if (!string.IsNullOrEmpty(endDateTimeString))
            {
                DateTime endDateTime;
                try
                {
                    endDateTime = DateTime.Parse(endDateTimeString, provider);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Задана некорректная дата окончания встречи. Операция изменения встречи отменена");
                    return false;
                }
                if (endDateTime <= meetingForUpdate.StartDateTime)
                {
                    Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция изменения встречи отменена");
                    return false;
                }
                meetingForUpdate.EndDateTime = endDateTime;
            }
            return true;
        }

        private bool TryUpdateNotificationTimeInMinute(Meeting meetingFromDb, Meeting meetingForUpdate)
        {
            var command = $"Старое значение:{meetingFromDb.MeetingNotificationTimeInMinutes}\n" +
                $"Для сохранения старого значения оставьте пустую строку и нажмите Enter\n" +
                $"Введите новое время напоминания о встрече в минутах: ";
            
            _consoleCommandHandler.WriteCommandToConsole(command);
            var notificationTimeString = Console.ReadLine();
            if (!string.IsNullOrEmpty(notificationTimeString))
            {
                if (!int.TryParse(notificationTimeString, out int notificationTime))
                {
                    Console.WriteLine("Задано некорректное время напоминания о встрече. Операция изменения встречи отменена");
                    return false;
                }
                if (notificationTime < 0)
                {
                    Console.WriteLine("Время напоминания о встрече не должно быть отрицательным. Операция создания новой встречи отменена");
                    return false;
                }

                meetingForUpdate.MeetingNotificationTimeInMinutes = notificationTime;
            }
            return true;
        }

        private async Task<bool> CheckIntersectMeetingDatesAsync(Meeting meetingForUpdate)
        {
            var isMeetingDatesIntersect = await _repository.CheckIntersectMeetingDatesAsync(meetingForUpdate);
            if (isMeetingDatesIntersect)
            {
                Console.WriteLine("Время измененной встречи пересекается с уже сущестующей встречей. Операция создания новой встречи отменена");
                return false;
            }
            return true;
        }
    }
}
