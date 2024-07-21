using PersonalMeetingsManager.DAL.Models;
using PersonalMeetingsManager.DAL.Repository;
using System.Globalization;

namespace PersonalMeetingsManager.Domain.CommandHandlers
{
    internal class AddMeetingCommandHandler
    {
        private readonly IMeetingsRepository _repository;
        private readonly ILastCommandInfo _lastCommandInfo;
        private readonly IConsoleCommandHandler _consoleCommandHandler;
        //private string _command;
        private readonly IFormatProvider provider = new CultureInfo("ru-RU");

        internal AddMeetingCommandHandler(IMeetingsRepository repository,
                                          ILastCommandInfo lastCommandInfo,
                                          IConsoleCommandHandler consoleCommandHandler)
        {
            _repository = repository;
            _lastCommandInfo = lastCommandInfo;
            _consoleCommandHandler = consoleCommandHandler;
        }

        internal async Task AddMeetingAsync()
        {
            var newMeeting = new Meeting();
            
            if (!TryAddSubject(out var subject))
                return;
            newMeeting.Subject = subject;

            if(!TryAddStartDateTime(out var startDateTime))
                return;
            newMeeting.StartDateTime = startDateTime;

            if(!TryAddFinishDateTime(newMeeting.StartDateTime, out var finishDateTime))
                return;
            newMeeting.EndDateTime = finishDateTime;

            if(!TryAddNotificationTimeInMinute(out var notificationTime))
                return;
            newMeeting.MeetingNotificationTimeInMinutes = notificationTime;

            if(!await CheckIntersectMeetingDatesAsync(newMeeting))
                return;

            var meeting = await _repository.AddMeetingAsync(newMeeting);
            Console.WriteLine("Создана новая встреча:");
            Console.WriteLine(MeetingInfoFormatter.GetMeetingInfoString(meeting));
        }

        private bool TryAddSubject(out string result)
        {
            var command = "Введите тему встречи: ";
            //WriteCommandToConsole(/*"Введите тему встречи: "*/command);
            _consoleCommandHandler.WriteCommandToConsole(command);
            var subject = Console.ReadLine();
            subject = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(/*_command*/command, subject);

            if (string.IsNullOrWhiteSpace(subject))
            {
                Console.WriteLine("Тема встречи не может быть пустой. Операция создания новой встречи отменена");
                result = default;
                return false;
            }
            result = subject;
            return true;
        }

        private bool TryAddStartDateTime(out DateTime result)
        {
            var command = "Введите время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            //WriteCommandToConsole(/*"Введите время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: "*/command);
            _consoleCommandHandler.WriteCommandToConsole(command);
            var startDateTimeString = Console.ReadLine();

            startDateTimeString = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(/*_command*/command, startDateTimeString);

            DateTime startDateTime;
            try
            {
                startDateTime = DateTime.Parse(startDateTimeString, provider);
            }
            catch (FormatException)
            {
                Console.WriteLine("Задана некорректная дата начала встречи. Операция создания новой встречи отменена");
                result = default;
                return false;
            }
            if (startDateTime <= DateTime.Now)
            {
                Console.WriteLine("Встреча может быть назначена только на будущее время. Операция создания новой встречи отменена");
                result = default;
                return false;
            }
            result = startDateTime;
            return true;
        }

        private bool TryAddFinishDateTime(DateTime startDateTime, out DateTime result)
        {
            var command = "Введите время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            //WriteCommandToConsole(/*"Введите время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: "*/command);
            _consoleCommandHandler.WriteCommandToConsole(command);
            var endDateTimeString = Console.ReadLine();
            endDateTimeString = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(/*_command*/command, endDateTimeString);

            DateTime finishDateTime;
            try
            {
                finishDateTime = DateTime.Parse(endDateTimeString, provider);
            }
            catch (FormatException)
            {
                Console.WriteLine("Задана некорректная дата окончания встречи. Операция создания новой встречи отменена");
                result = default;
                return false;
            }
            if (finishDateTime <= startDateTime)
            {
                Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция создания новой встречи отменена");
                result = default;
                return false;
            }
            result = finishDateTime;
            return true;
        }

        private bool TryAddNotificationTimeInMinute(out int result)
        {
            var command = "Введите время напоминания о встрече в минутах: ";
            //WriteCommandToConsole(/*"Введите время напоминания о встрече в минутах: "*/ command);
            _consoleCommandHandler.WriteCommandToConsole(command);
            var notificationTimeString = Console.ReadLine();
            notificationTimeString = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(/*_command*/command, notificationTimeString);

            if (!int.TryParse(notificationTimeString, out int notificationTime))
            {
                Console.WriteLine("Задано некорректное время напоминания о встрече. Операция создания новой встречи отменена");
                result = default;
                return false;
            }
            if (notificationTime < 0)
            {
                Console.WriteLine("Время напоминания о встрече не должно быть отрицательным. Операция создания новой встречи отменена");
                result = default;
                return false;
            }
            result = notificationTime;
            return true;
        }

        private async Task<bool> CheckIntersectMeetingDatesAsync(Meeting newMeeting)
        {
            var isMeetingDatesIntersect = await _repository.CheckIntersectMeetingDatesAsync(newMeeting);
            if (isMeetingDatesIntersect)
            {
                Console.WriteLine("Время новой встречи пересекается с уже сущестующей встречей. Операция создания новой встречи отменена");
                return false;
            }
            return true;
        }

        //private void WriteCommandToConsole(string command)
        //{
        //    /*_command = command;*/
        //    _lastCommandInfo.LastCommand = /*_command*/command;
        //    Console.Write(/*_command*/command);

        //    var (Left, _) = Console.GetCursorPosition();
        //    _lastCommandInfo.LastPosition = Left;
        //}
    }
}
