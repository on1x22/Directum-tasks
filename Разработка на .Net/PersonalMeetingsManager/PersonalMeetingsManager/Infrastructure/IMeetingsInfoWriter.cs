using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Infrastructure
{
    internal interface IMeetingsInfoWriter
    {
        Task Write(List<Meeting> meetings);
    }
}
