using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Domain
{
    internal interface IConsoleCommandHandler
    {
        string RemoveOldTextAfterMoveToNewLine(string command, string stringInfo);
        void WriteCommandToConsole(string command);
        //void WriteNotificationOnNewLine(string message/*, DateTime nowDateTime*/);
        //void WriteStartMeetingNotificationOnNewLine(Meeting meeting);
        //void WriteNotificationOnNewLine(Meeting meeting);
        //void WriteNotificationOnNewLine(Meeting meeting, TimeSpan remainingTime);
        void WriteNotificationOnNewLine(NotificationType type, Meeting meeting, TimeSpan remainingTime, bool isFirstStart);
    }
}
