using Microsoft.EntityFrameworkCore;
using PersonalMeetingsManager.DAL.Contexts;
using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.DAL.Repository
{
    internal class MeetingsRepository : IMeetingsRepository
    {
        private readonly MeetingsDbContext _dbContext;
        
        public MeetingsRepository(MeetingsDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task<List<Meeting>> GetMeetingsByDayAsync(DateTime dateTime) =>    
            await _dbContext.Meetings.Where(m => m.StartDateTime.Date <= dateTime.Date && 
                m.EndDateTime.Date >= dateTime.Date).ToListAsync();

        public async Task<Meeting> GetMeetingByIdAsync(int meetingId)
        {
            var meeting = await _dbContext.Meetings.FirstOrDefaultAsync(m => m.Id == meetingId);
            if (meeting == null)
                return default;
            return meeting;
        }

        public async Task<List<Meeting>> GetUpcomingMeetings(DateTime dateTime) =>
            await _dbContext.Meetings.Where(m => m.StartDateTime >= dateTime).ToListAsync();

        public async Task<Meeting> AddMeetingAsync(Meeting meeting)
        {
            await _dbContext.Meetings.AddAsync(meeting);
            await SaveChangesAsync();
            return meeting;
        }

        public async Task<Meeting> UpdateMeetingAsync(Meeting meeting)
        {
            var meetingForUpdate = await _dbContext.Meetings.FirstOrDefaultAsync(m => m.Id == meeting.Id);

            meetingForUpdate!.Subject = meeting.Subject;
            meetingForUpdate.StartDateTime = meeting.StartDateTime;
            meetingForUpdate.EndDateTime = meeting.EndDateTime;
            meetingForUpdate.MeetingNotificationTimeInMinutes = meeting.MeetingNotificationTimeInMinutes;

            await SaveChangesAsync();
            return meetingForUpdate;
        }

        public async Task<Meeting> DeleteMeetingAsync(int id)
        {
            var meeting =  await _dbContext.Meetings.FirstOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
                return default;

            _dbContext.Meetings.Remove(meeting);
            await SaveChangesAsync();
            return meeting;
        }
        public async Task<bool> CheckIntersectMeetingDatesAsync(Meeting meeting)
        {
            var intersectedMeetings = await _dbContext.Meetings
                .Where(m => ((meeting.StartDateTime >= m.StartDateTime && meeting.StartDateTime < m.EndDateTime) ||
                             (meeting.EndDateTime > m.StartDateTime && meeting.EndDateTime <= m.EndDateTime) ||
                             (meeting.StartDateTime <= m.StartDateTime && meeting.EndDateTime >= m.EndDateTime)) &&
                             meeting.Id != m.Id).ToListAsync();

            if (intersectedMeetings.Count > 0)
                return true;
            
            return false;
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();        
    }
}
