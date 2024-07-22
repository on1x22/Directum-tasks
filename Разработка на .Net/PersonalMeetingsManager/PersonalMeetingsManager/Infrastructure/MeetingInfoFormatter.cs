using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Infrastructure
{
    internal static class MeetingInfoFormatter
    {
        internal static string GetMeetingInfoString(Meeting meeting)
        {
            string mainInfo = $"- {meeting.Subject} (Id={meeting.Id}) с {meeting.StartDateTime} " +
                            $"по {meeting.EndDateTime}. ";

            string notificationInfo = string.Empty;
            if (meeting.MeetingNotificationTimeInMinutes > 0)
                notificationInfo = $"Уведомление за {meeting.MeetingNotificationTimeInMinutes}" +
                    $" минут до встречи.";
            else
                notificationInfo = $"Уведомление о встрече отсутствует.";

            return mainInfo + notificationInfo;
        }

        internal static string GetMeetingNotificationString(Meeting meeting)
        {
            return $"Внимание! Через {meeting.MeetingNotificationTimeInMinutes} минут " +
                    $"начнется встреча \"{meeting.Subject}\" (Id={meeting.Id}) с {meeting.StartDateTime}" +
                    $" по {meeting.EndDateTime}. Напоминание за " +
                    $"{meeting.MeetingNotificationTimeInMinutes} минут";
        }

        internal static string GetMeetingNotificationString(Meeting meeting, TimeSpan remainingTime)
        {
            return $"Внимание! Через {meeting.MeetingNotificationTimeInMinutes +
                    remainingTime.Minutes} минут начнется встреча \"{meeting.Subject}\" (Id={meeting.Id}) " +
                    $"с {meeting.StartDateTime} по {meeting.EndDateTime}. " +
                    $"Напоминание за {meeting.MeetingNotificationTimeInMinutes} минут";
        }

        internal static string GetStartOfMeetingString(Meeting meeting)
        {
            return $"ВСТРЕЧА \"{meeting.Subject}\" (ID={meeting.Id}) НАЧАЛАСЬ. " +
                    $"ВРЕМЯ НАЧАЛА {meeting.StartDateTime}, ВРЕМЯ ОКОНЧАНИЯ " +
                    $"{meeting.EndDateTime}, НАПОМИНАНИЕ ЗА " +
                    $"{meeting.MeetingNotificationTimeInMinutes} МИНУТ";
        }
    }
}
