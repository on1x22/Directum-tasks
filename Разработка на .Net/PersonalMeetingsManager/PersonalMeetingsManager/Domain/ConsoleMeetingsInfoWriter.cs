using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Domain
{
    internal class ConsoleMeetingsInfoWriter : IMeetingsInfoWriter
    {
        public Task Write(List<Meeting> meetings)
        {
            foreach (var meeting in meetings)
            {
                Console.WriteLine(MeetingInfoFormatter.GetMeetingInfoString(meeting));
            }
            return Task.CompletedTask;
        }
    }
}
