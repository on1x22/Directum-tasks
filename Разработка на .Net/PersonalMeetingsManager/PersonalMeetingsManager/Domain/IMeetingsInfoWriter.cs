using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Domain
{
    internal interface IMeetingsInfoWriter
    {
        Task Write(List<Meeting> meetings);
    }
}
