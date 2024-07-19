using Microsoft.EntityFrameworkCore;
using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.DAL.Contexts
{
    public class MeetingsDbContext : DbContext
    {
        public MeetingsDbContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            var dateTimeNow = DateTime.Now;
            modelBuilder.Entity<Meeting>().HasData(                
                [
                    new Meeting()
                    {
                        Id = 1,
                        Subject = "Совещание которое уже прошло 2 дня назад",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-2),
                            new TimeOnly(8, 5)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-2),
                            new TimeOnly(8, 30)
                            ),
                        MeetingNotificationTimeInMinutes = 0
                    },
                    new Meeting()
                    {
                        Id = 2,
                        Subject = "Daily прошло 2 дня назад",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-2),
                            new TimeOnly(10, 30)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-2),
                            new TimeOnly(12, 0)
                            ),
                        MeetingNotificationTimeInMinutes = 15
                    },
                    new Meeting()
                    {
                        Id = 3,
                        Subject = "Daily прошло вчера",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-1), 
                            new TimeOnly(10, 30)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-1),
                            new TimeOnly(12, 0)
                            ),
                        MeetingNotificationTimeInMinutes = 15
                    },
                    new Meeting()
                    {
                        Id = 4,
                        Subject = "New meeting прошел вчера",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-1),
                            new TimeOnly(22, 0)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(-1),
                            new TimeOnly(22, 20)
                            ),
                        MeetingNotificationTimeInMinutes = 5
                    },
                    new Meeting()
                    {
                        Id = 5,
                        Subject = "Очень важное совещание сегодня",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day), 
                            new TimeOnly(10, 0)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day), 
                            new TimeOnly(13, 0)
                            ),
                        MeetingNotificationTimeInMinutes = 15
                    },
                    new Meeting()
                    {
                        Id = 6,
                        Subject = "Обсуждение результатов работы завтра",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(1),
                            new TimeOnly(9, 20)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(1),
                            new TimeOnly(15, 33)
                            ),
                        MeetingNotificationTimeInMinutes = 0
                    },
                    new Meeting()
                    {
                        Id = 7,
                        Subject = "Встреча, начавшаяся в прошлый день и закончившаяся в текущий",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year, 
                                         dateTimeNow.Month, 
                                         dateTimeNow.Day).AddDays(-1), 
                            new TimeOnly(23, 0)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year, 
                                         dateTimeNow.Month, 
                                         dateTimeNow.Day), 
                            new TimeOnly(2, 15)
                            ),
                        MeetingNotificationTimeInMinutes = 0
                    },
                    new Meeting()
                    {
                        Id = 8,
                        Subject = "Встреча, начавшаяся в текущий день и заканчивающаяся на следующий",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day),
                            new TimeOnly(22, 10)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(1),
                            new TimeOnly(1, 0)
                            ),
                        MeetingNotificationTimeInMinutes = 0
                    },
                    new Meeting()
                    {
                        Id = 9,
                        Subject = "Встреча, длящаяся 3 дня",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(10),
                            new TimeOnly(22, 10)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(12),
                            new TimeOnly(1, 0)
                            ),
                        MeetingNotificationTimeInMinutes = 0
                    },
                ]);
        }
    }
}
