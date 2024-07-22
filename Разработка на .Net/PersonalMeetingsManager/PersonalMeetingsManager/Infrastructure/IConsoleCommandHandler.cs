using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Infrastructure
{
    internal interface IConsoleCommandHandler
    {
        string RemoveOldTextAfterMoveToNewLine(string command, string stringInfo);
        void WriteCommandToConsole(string command);
        void WriteNotificationOnNewLine(NotificationType type, Meeting meeting, TimeSpan remainingTime, bool isFirstStart);
    }
}
