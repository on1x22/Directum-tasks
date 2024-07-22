using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Infrastructure
{
    internal class ConsoleCommandHandler : IConsoleCommandHandler
    {
        private readonly ILastCommandInfo _lastCommandInfo;

        public ConsoleCommandHandler(ILastCommandInfo lastCommandInfo) =>
            _lastCommandInfo = lastCommandInfo;


        public string RemoveOldTextAfterMoveToNewLine(string command, string stringInfo)
        {
            var strLength = _lastCommandInfo.LastPosition - command.Length;
            if (strLength > 0)
                return stringInfo.Substring(strLength);

            return stringInfo;
        }

        public void WriteCommandToConsole(string command)
        {
            _lastCommandInfo.LastCommand = command;
            Console.Write(command);

            var (Left, _) = Console.GetCursorPosition();
            _lastCommandInfo.LastPosition = Left;
        }

        public void WriteNotificationOnNewLine(NotificationType type, Meeting meeting, TimeSpan remainingTime, bool isFirstStart)
        {
            var (Left, _) = Console.GetCursorPosition();
            if (Left >= 0)
            {
                switch (type)
                {
                    case NotificationType.Meeting:
                        Console.WriteLine();
                        Console.WriteLine(MeetingInfoFormatter.GetStartOfMeetingString(meeting));

                        break;
                    case NotificationType.Notification:
                        WriteNotificationOnNewLine(meeting, remainingTime, isFirstStart);
                        break;
                }
            }
        }

        public void WriteNotificationOnNewLine(Meeting meeting, TimeSpan remainingTime, bool isFirstStart)
        {
            if (isFirstStart == true && remainingTime.TotalMinutes < 0)
            {
                Console.WriteLine();
                Console.WriteLine(MeetingInfoFormatter.GetMeetingNotificationString(meeting, remainingTime));
            }

            if (remainingTime.TotalMinutes == 0)
            {
                Console.WriteLine();
                Console.WriteLine(MeetingInfoFormatter.GetMeetingNotificationString(meeting));
            }
        }
    }

    internal enum NotificationType
    {
        Notification,
        Meeting
    }
}
