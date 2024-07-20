using PersonalMeetingsManager.DAL.Models;
using PersonalMeetingsManager.DAL.Repository;
using System.Globalization;

namespace PersonalMeetingsManager.Domain.CommandHandlers
{
    internal class UpdateMeetingCommandHandler
    {
        private readonly IMeetingsRepository _repository;

        internal UpdateMeetingCommandHandler(IMeetingsRepository repository)
        {
            _repository = repository;
        }
        internal async Task UpdateMeetingAsync(string meetingIdString)
        {
            IFormatProvider provider = new CultureInfo("ru-RU");
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

            Console.WriteLine("Старое значение: " + meetingFromDb.Subject);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новую тему встречи: ");
            var subject = Console.ReadLine();
            if (!string.IsNullOrEmpty(subject))
                meetingForUpdate.Subject = subject;

            Console.WriteLine("Старое значение: " + meetingFromDb.StartDateTime);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новое время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
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
                    return;
                }
                if (startDateTime <= DateTime.Now)
                {
                    Console.WriteLine("Встреча может быть назначена только на будущее время. Операция изменения встречи отменена");
                    return;
                }
                meetingForUpdate.StartDateTime = startDateTime;
            }

            Console.WriteLine("Старое значение: " + meetingFromDb.EndDateTime);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новое время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
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
                    return;
                }
                if (endDateTime <= meetingForUpdate.StartDateTime)
                {
                    Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция изменения встречи отменена");
                    return;
                }
                meetingForUpdate.EndDateTime = endDateTime;
            }

            Console.WriteLine("Старое значение: " + meetingFromDb.MeetingNotificationTimeInMinutes);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новое время напоминания о встрече в минутах: ");
            var notificationTimeString = Console.ReadLine();
            if (!string.IsNullOrEmpty(notificationTimeString))
            {
                if (!int.TryParse(notificationTimeString, out int notificationTime))
                {
                    Console.WriteLine("Задано некорректное время напоминания о встрече. Операция изменения встречи отменена");
                    return;
                }

                meetingForUpdate.MeetingNotificationTimeInMinutes = notificationTime;
            }

            var isMeetingDatesIntersect = await _repository.CheckIntersectMeetingsDatesAsync(meetingForUpdate);
            if (isMeetingDatesIntersect)
            {
                Console.WriteLine("Время измененной встречи пересекается с уже сущестующей встречей. Операция создания новой встречи отменена");
                return;
            }

            var updatedMeeting = await _repository.UpdateMeetingAsync(meetingForUpdate);
            Console.WriteLine("Встреча изменена:");
            Console.WriteLine(MeetingInfoFormatter.GetMeetingInfoString(updatedMeeting));
        }
    }
}
