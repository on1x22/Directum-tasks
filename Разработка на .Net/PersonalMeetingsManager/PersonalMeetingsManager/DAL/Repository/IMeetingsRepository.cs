using PersonalMeetingsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsManager.DAL.Repository
{
    internal interface IMeetingsRepository
    {
        Task<List<Meeting>> GetMeetingsByDayAsync(DateTime dateTime);
        Task<Meeting> GetMeetingByIdAsync(int meetingId);
        Task<List<Meeting>> GetUpcomingMeetings(DateTime dateTime);
        Task<Meeting> AddMeetingAsync(Meeting meeting);
        Task<Meeting> UpdateMeetingAsync(Meeting meeting);
        Task<Meeting> DeleteMeetingAsync(int id);
        Task<bool> CheckIntersectMeetingDatesAsync(Meeting meeting);
    }
}
