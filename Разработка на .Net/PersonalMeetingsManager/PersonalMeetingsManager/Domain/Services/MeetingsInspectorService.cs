using PersonalMeetingsManager.DAL.Models;
using PersonalMeetingsManager.DAL.Repository;
using PersonalMeetingsManager.Infrastructure;

namespace PersonalMeetingsManager.Domain.Services
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

                var (LeftBeforeNotification, TopBeforeNotifications) = Console.GetCursorPosition();
                _lastCommandInfo.LastPosition = LeftBeforeNotification;
                foreach (var meeting in actualMeetings)
                {
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
                await Task.Delay(ONE_MINUTE);
            }
        }

        private void InspectNotifications(Meeting meeting, DateTime dtNow, bool isFirstStart)
        {
            var remainingTime = meeting.StartDateTime
                .AddMinutes(-meeting.MeetingNotificationTimeInMinutes) - dtNow;
            if (remainingTime.TotalMinutes > 0)
                return;

            _consoleCommandHandler.WriteNotificationOnNewLine(NotificationType.Notification, meeting,
                                                              remainingTime, isFirstStart);
        }

        private void InspectStartOfMeetings(Meeting meeting, DateTime dtNow)
        {
            var remainingTime = meeting.StartDateTime - dtNow;
            if (remainingTime.TotalMinutes == 0)
            {
                _consoleCommandHandler.WriteNotificationOnNewLine(NotificationType.Meeting, meeting,
                                                                  remainingTime, isFirstStart: false);
            }
        }
    }
}
