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
            var dateTime5MinutesAfterNow = dateTimeNow.AddMinutes(5);
            var dateTime16MinutesAfterNow = dateTimeNow.AddMinutes(16);
            var dateTime30MinutesAfterNow = dateTimeNow.AddMinutes(30);
            var dateTime40MinutesAfterNow = dateTimeNow.AddMinutes(40);
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
                        Subject = "Совещание, начинающееся через 5 минут с напоминанием за 6 минут",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTime5MinutesAfterNow.Year,
                                         dateTime5MinutesAfterNow.Month,
                                         dateTime5MinutesAfterNow.Day),
                            new TimeOnly(dateTime5MinutesAfterNow.Hour,
                                         dateTime5MinutesAfterNow.Minute)
                            ),
                        EndDateTime = new DateTime(                            
                            new DateOnly(dateTime5MinutesAfterNow.AddMinutes(5).Year,
                                         dateTime5MinutesAfterNow.AddMinutes(5).Month,
                                         dateTime5MinutesAfterNow.AddMinutes(5).Day),
                            new TimeOnly(dateTime5MinutesAfterNow.AddMinutes(5).Hour,
                                         dateTime5MinutesAfterNow.AddMinutes(5).Minute)
                            ),
                        MeetingNotificationTimeInMinutes = 6
                    },
                    new Meeting()
                    {
                        Id = 6,
                        Subject = "Совещание, начинающееся через 16 минут",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTime16MinutesAfterNow.Year,
                                         dateTime16MinutesAfterNow.Month,
                                         dateTime16MinutesAfterNow.Day), 
                            new TimeOnly(dateTime16MinutesAfterNow.Hour, 
                                         dateTime16MinutesAfterNow.Minute)
                            ),
                        EndDateTime = new DateTime(                           
                            new DateOnly(dateTime16MinutesAfterNow.AddMinutes(7).Year,
                                         dateTime16MinutesAfterNow.AddMinutes(7).Month,
                                         dateTime16MinutesAfterNow.AddMinutes(7).Day),
                            new TimeOnly(dateTime16MinutesAfterNow.AddMinutes(7).Hour,
                                         dateTime16MinutesAfterNow.AddMinutes(7).Minute)
                            ),
                        MeetingNotificationTimeInMinutes = 15
                    },
                    new Meeting()
                    {
                        Id = 7,
                        Subject = "Совещание, начинающееся через 30 минут",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTime30MinutesAfterNow.Year,
                                         dateTime30MinutesAfterNow.Month,
                                         dateTime30MinutesAfterNow.Day),
                            new TimeOnly(dateTime30MinutesAfterNow.Hour,
                                         dateTime30MinutesAfterNow.Minute)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTime30MinutesAfterNow.AddMinutes(10).Year,
                                         dateTime30MinutesAfterNow.AddMinutes(10).Month,
                                         dateTime30MinutesAfterNow.AddMinutes(10).Day),
                            new TimeOnly(dateTime30MinutesAfterNow.AddMinutes(10).Hour,
                                         dateTime30MinutesAfterNow.AddMinutes(10).Minute)
                            ),
                        MeetingNotificationTimeInMinutes = 29
                    },
                    new Meeting()
                    {
                        Id = 8,
                        Subject = "Совещание, начинающееся через 40 минут",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTime40MinutesAfterNow.Year,
                                         dateTime40MinutesAfterNow.Month,
                                         dateTime40MinutesAfterNow.Day),
                            new TimeOnly(dateTime40MinutesAfterNow.Hour,
                                         dateTime40MinutesAfterNow.Minute)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTime40MinutesAfterNow.AddMinutes(5).Year,
                                         dateTime40MinutesAfterNow.AddMinutes(5).Month,
                                         dateTime40MinutesAfterNow.AddMinutes(5).Day),
                            new TimeOnly(dateTime40MinutesAfterNow.AddMinutes(5).Hour,
                                         dateTime40MinutesAfterNow.AddMinutes(5).Minute)
                            ),
                        MeetingNotificationTimeInMinutes = 37
                    },                    
                    new Meeting()
                    {
                        Id = 9,
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
                        Id = 10,
                        Subject = "Встреча, начинающаяся завтра и заканчивающаяся послезавтра",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year, 
                                         dateTimeNow.Month, 
                                         dateTimeNow.Day).AddDays(1), 
                            new TimeOnly(23, 0)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year, 
                                         dateTimeNow.Month, 
                                         dateTimeNow.Day).AddDays(2), 
                            new TimeOnly(2, 15)
                            ),
                        MeetingNotificationTimeInMinutes = 0
                    },
                    new Meeting()
                    {
                        Id = 11,
                        Subject = "Встреча, начинающаяся послезавтра и заканчивающаяся через 2 дня",
                        StartDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(2),
                            new TimeOnly(22, 10)
                            ),
                        EndDateTime = new DateTime(
                            new DateOnly(dateTimeNow.Year,
                                         dateTimeNow.Month,
                                         dateTimeNow.Day).AddDays(4),
                            new TimeOnly(1, 0)
                            ),
                        MeetingNotificationTimeInMinutes = 0
                    },
                    new Meeting()
                    {
                        Id = 12,
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
