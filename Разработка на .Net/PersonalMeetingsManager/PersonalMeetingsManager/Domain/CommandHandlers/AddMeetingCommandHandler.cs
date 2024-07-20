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
        private string _command;

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
            IFormatProvider provider = new CultureInfo("ru-RU");

            /*_command = "Введите тему встречи: ";
            _lastCommandInfo.LastCommand = _command;
            Console.Write(_command);
            var (Left, _) = Console.GetCursorPosition();
            _lastCommandInfo.LastPosition = Left;*/

            WriteCommandToConsole("Введите тему встречи: ");
            var subject = Console.ReadLine();
            subject = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(_command, subject);

            if (string.IsNullOrWhiteSpace(subject))
            {
                Console.WriteLine("Тема встречи не может быть пустой. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.Subject = subject;

            /*_command = "Введите время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            _lastCommandInfo.LastCommand = _command;
            Console.Write(_command);
            (Left, _) = Console.GetCursorPosition();
            _lastCommandInfo.LastPosition = Left;*/
            WriteCommandToConsole("Введите время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
            var startDateTimeString = Console.ReadLine();
            //var (Left, Top) = Console.GetCursorPosition();
            /*var strLength = _lastCommandInfo.LastPosition - _command.Length;
            if (strLength > 0)
            {
                startDateTimeString = startDateTimeString.Substring(strLength);
            }*/
            startDateTimeString = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(_command, startDateTimeString);

            DateTime startDateTime;
            try
            {
                startDateTime = DateTime.Parse(startDateTimeString, provider);
            }
            catch (FormatException)
            {
                Console.WriteLine("Задана некорректная дата начала встречи. Операция создания новой встречи отменена");
                return;
            }
            if (startDateTime <= DateTime.Now)
            {
                Console.WriteLine("Встреча может быть назначена только на будущее время. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.StartDateTime = startDateTime;

            /*_command = "Введите время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ";
            _lastCommandInfo.LastCommand = _command;
            Console.Write(_command);
            (Left, _) = Console.GetCursorPosition();
            _lastCommandInfo.LastPosition = Left;*/
            WriteCommandToConsole("Введите время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
            var endDateTimeString = Console.ReadLine();
            endDateTimeString = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(_command, endDateTimeString);

            DateTime endDateTime;
            try
            {
                endDateTime = DateTime.Parse(endDateTimeString, provider);
            }
            catch (FormatException)
            {
                Console.WriteLine("Задана некорректная дата окончания встречи. Операция создания новой встречи отменена");
                return;
            }
            if (endDateTime <= newMeeting.StartDateTime)
            {
                Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.EndDateTime = endDateTime;

            /*_command = "Введите время напоминания о встрече в минутах: ";
            _lastCommandInfo.LastCommand = _command;
            Console.Write(_command);
            (Left, _) = Console.GetCursorPosition();
            _lastCommandInfo.LastPosition = Left;*/
            WriteCommandToConsole("Введите время напоминания о встрече в минутах: ");
            var notificationTimeString = Console.ReadLine();
            notificationTimeString = _consoleCommandHandler
                .RemoveOldTextAfterMoveToNewLine(_command, notificationTimeString);

            if (!int.TryParse(notificationTimeString, out int notificationTime))
            {
                Console.WriteLine("Задано некорректное время напоминания о встрече. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.MeetingNotificationTimeInMinutes = notificationTime;

            var isMeetingDatesIntersect = await _repository.CheckIntersectMeetingsDatesAsync(newMeeting);
            if (isMeetingDatesIntersect)
            {
                Console.WriteLine("Время новой встречи пересекается с уже сущестующей встречей. Операция создания новой встречи отменена");
                return;
            }

            var meeting = await _repository.AddMeetingAsync(newMeeting);
            Console.WriteLine("Создана новая встреча:");
            Console.WriteLine(MeetingInfoFormatter.GetMeetingInfoString(meeting));
        }

        private void WriteCommandToConsole(string command)
        {
            _command = command;
            _lastCommandInfo.LastCommand = _command;
            Console.Write(_command);
            /*var*/
            var (Left, _) = Console.GetCursorPosition();
            _lastCommandInfo.LastPosition = Left;
        }
    }
}
