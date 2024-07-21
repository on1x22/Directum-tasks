using PersonalMeetingsManager.DAL.Models;
using PersonalMeetingsManager.DAL.Repository;

namespace PersonalMeetingsManager.Domain
{
    internal class MeetingsInspectorService : IMeetingsInspectorService
    {
        private readonly IMeetingsRepository _repository;
        private readonly ILastCommandInfo _lastCommandInfo;
        private readonly IConsoleCommandHandler _consoleCommandHandler;
        private const int ONE_MINUTE = 60 * 1000;

        public MeetingsInspectorService(IMeetingsRepository repository,
                                        ILastCommandInfo lastCommandInfo,
                                        IConsoleCommandHandler consoleCommandHandler) 
        {
            _repository = repository;
            _lastCommandInfo = lastCommandInfo;
            _consoleCommandHandler = consoleCommandHandler;
        }

        public async Task InspectMeetingsAsync()
        {
            bool isFirstStart = true;

            while (true)
            {
                var nowDateTime = DateTime.Now;
                var dateTimeWithoutSeconds = new DateTime(
                    nowDateTime.Year, 
                    nowDateTime.Month, 
                    nowDateTime.Day, 
                    nowDateTime.Hour, 
                    nowDateTime.Minute, 
                    0);
                var actualMeetings = await _repository.GetUpcomingMeetings(dateTimeWithoutSeconds);

                //_consoleCommandHandler.WriteNotificationOnNewLine($"Текущее время: {nowDateTime}");
                /*var (Left, _) = Console.GetCursorPosition();
                if (Left > 0)
                {
                    _lastCommandHandler.LastPosition = Left;
                    Console.WriteLine();
                    Console.WriteLine("Текущее время: " + nowDateTime);
                    Console.WriteLine();
                    Console.Write(_lastCommandHandler.LastCommand);
                }*/
                //Console.WriteLine("Текущее время: " + nowDateTime);


                //var dt = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, 0);

                var (LeftBeforeNotification, TopBeforeNotifications) = Console.GetCursorPosition();
                _lastCommandInfo.LastPosition =LeftBeforeNotification;
                foreach (var meeting in actualMeetings)
                {
                    /*var remainingTime = meeting.StartDateTime.AddMinutes(-meeting.NotificationTime) - dt;
                    if (isFirstStart == true && remainingTime.Minutes < 0)
                    {
                        Console.WriteLine($"Внимание! Через {meeting.NotificationTime + remainingTime.Minutes} минут начнется встреча {meeting.Id} с {meeting.StartDateTime}. Напоминание за {meeting.NotificationTime} минут");               
                    }

                    if (remainingTime.Minutes == 0)
                        Console.WriteLine($"Внимание! Через {meeting.NotificationTime} минут начнется встреча {meeting.Id} с {meeting.StartDateTime}. Напоминание за {meeting.NotificationTime} минут");*/
                    InspectNotifications(meeting, dateTimeWithoutSeconds, isFirstStart);
                    InspectStartOfMeetings(meeting, dateTimeWithoutSeconds);
                }
                
                var (_, TopAfterNotifications) = Console.GetCursorPosition();
                if (TopBeforeNotifications < TopAfterNotifications)
                {
                    Console.WriteLine();
                    Console.Write(_lastCommandInfo.LastCommand);
                }

                isFirstStart = false;
                //Console.WriteLine("Test message from thread");
                //Thread.Sleep(ONE_MINUTE);
                await Task.Delay(ONE_MINUTE);
            }
        }

        private void InspectNotifications(Meeting meeting, DateTime dtNow, bool isFirstStart)
        {
            var remainingTime = meeting.StartDateTime
                .AddMinutes(-meeting.MeetingNotificationTimeInMinutes) - dtNow;
            if (remainingTime.TotalMinutes > 0) 
                return;
            //if (isFirstStart == true && remainingTime./*Minutes*/TotalMinutes < 0)
            //{
                /*Console.WriteLine($"Внимание! Через {meeting.MeetingNotificationTimeInMinutes + 
                    remainingTime.Minutes} минут начнется встреча \"{meeting.Subject}\" (Id={meeting.Id}) " +
                    $"с {meeting.StartDateTime} по {meeting.EndDateTime}." +
                    $" Напоминание за {meeting.MeetingNotificationTimeInMinutes} минут");*/
                //Console.WriteLine(MeetingInfoFormatter.GetMeetingNotificationString(meeting, remainingTime));
            //    _consoleCommandHandler.WriteStartMeetingNotificationOnNewLine(meeting);
            //}

            //if (remainingTime./*Minutes*/TotalMinutes == 0)
            //{
                /*Console.WriteLine($"Внимание! Через {meeting.MeetingNotificationTimeInMinutes} минут " +
                    $"начнется встреча {meeting.Id} с {meeting.StartDateTime}. Напоминание " +
                    $"за {meeting.MeetingNotificationTimeInMinutes} минут");*/
                //Console.WriteLine(MeetingInfoFormatter.GetMeetingNotificationString(meeting));
            //    _consoleCommandHandler.WriteStartMeetingNotificationOnNewLine(meeting);
            //}
            _consoleCommandHandler.WriteNotificationOnNewLine(NotificationType.Notification, meeting,
                                                              remainingTime, isFirstStart);
        }

        private void InspectStartOfMeetings(Meeting meeting, DateTime dtNow)
        {
            var remainingTime = meeting.StartDateTime - dtNow;
            if (remainingTime./*Minutes*/TotalMinutes == 0)
            {
                /*Console.WriteLine($"ВСТРЕЧА С ID={meeting.Id} НАЧАЛАСЬ. " +
                    $"ВРЕМЯ НАЧАЛА {meeting.StartDateTime}, НАПОМИНАНИЕ ЗА " +
                    $"{meeting.MeetingNotificationTimeInMinutes} МИНУТ");*/
                //Console.WriteLine(MeetingInfoFormatter.GetStartOfMeetingString(meeting));
                //_consoleCommandHandler.WriteStartMeetingNotificationOnNewLine(meeting);
                _consoleCommandHandler.WriteNotificationOnNewLine(NotificationType.Meeting, meeting,
                                                                  remainingTime, isFirstStart: false);
            }
        }

        private void __WriteNotificationOnNewLine(Meeting meeting)
        {
            var (Left, _) = Console.GetCursorPosition();
            if (Left > 0)
            {
                _lastCommandInfo.LastPosition = Left;
                Console.WriteLine();
                Console.WriteLine(MeetingInfoFormatter.GetStartOfMeetingString(meeting));
                Console.WriteLine();
                Console.Write(_lastCommandInfo.LastCommand);
            }
        }
    }
}
