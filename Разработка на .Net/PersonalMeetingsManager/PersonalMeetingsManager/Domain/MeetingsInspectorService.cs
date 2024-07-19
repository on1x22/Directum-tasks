using PersonalMeetingsManager.DAL.Models;
using PersonalMeetingsManager.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsManager.Domain
{
    internal class MeetingsInspectorService : IMeetingsInspectorService
    {
        private readonly IMeetingsRepository _repository;
        private const int ONE_MINUTE = 60 * 1000;

        public MeetingsInspectorService(IMeetingsRepository repository) 
        {
            _repository = repository;
        }

        public async Task InspectMeetings()
        {
            bool isFirstStart = true;

            while (true)
            {
                var nowDateTime = DateTime.Now;
                var dateTimeWithoutSeconds = new DateTime(
                    nowDateTime.Year, 
                    nowDateTime.Month, 
                    nowDateTime.Day, 
                    nowDateTime.Hour, 
                    nowDateTime.Minute, 
                    0);
                var actualMeetings = await _repository.GetUpcomingMeetings(dateTimeWithoutSeconds);

                //var nowDateTime = DateTime.Now;
                Console.WriteLine("Текущее время: " + nowDateTime);
                //var dt = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, 0);

                foreach (var meeting in actualMeetings)
                {
                    /*var remainingTime = meeting.StartDateTime.AddMinutes(-meeting.NotificationTime) - dt;
                    if (isFirstStart == true && remainingTime.Minutes < 0)
                    {
                        Console.WriteLine($"Внимание! Через {meeting.NotificationTime + remainingTime.Minutes} минут начнется встреча {meeting.Id} с {meeting.StartDateTime}. Напоминание за {meeting.NotificationTime} минут");               
                    }

                    if (remainingTime.Minutes == 0)
                        Console.WriteLine($"Внимание! Через {meeting.NotificationTime} минут начнется встреча {meeting.Id} с {meeting.StartDateTime}. Напоминание за {meeting.NotificationTime} минут");*/
                    InspectNotifications(meeting, dateTimeWithoutSeconds, isFirstStart);
                    InspectStartOfMeetings(meeting, dateTimeWithoutSeconds);
                }

                isFirstStart = false;
                Console.WriteLine("Test message from thread");
                //Thread.Sleep(ONE_MINUTE);
                await Task.Delay(ONE_MINUTE);
            }
        }

        private void InspectNotifications(Meeting meeting, DateTime dtNow, bool isFirstStart)
        {
            var remainingTime = meeting.StartDateTime
                .AddMinutes(-meeting.MeetingNotificationTimeInMinutes) - dtNow;
            if (isFirstStart == true && remainingTime.Minutes < 0)
            {
                /*Console.WriteLine($"Внимание! Через {meeting.MeetingNotificationTimeInMinutes + 
                    remainingTime.Minutes} минут начнется встреча \"{meeting.Subject}\" (Id={meeting.Id}) " +
                    $"с {meeting.StartDateTime} по {meeting.EndDateTime}." +
                    $" Напоминание за {meeting.MeetingNotificationTimeInMinutes} минут");*/
                Console.WriteLine(MeetingInfoFormatter.GetMeetingNotificationString(meeting, remainingTime));
            }

            if (remainingTime.Minutes == 0)
                /*Console.WriteLine($"Внимание! Через {meeting.MeetingNotificationTimeInMinutes} минут " +
                    $"начнется встреча {meeting.Id} с {meeting.StartDateTime}. Напоминание " +
                    $"за {meeting.MeetingNotificationTimeInMinutes} минут");*/
                Console.WriteLine(MeetingInfoFormatter.GetMeetingNotificationString(meeting));
        }

        private void InspectStartOfMeetings(Meeting meeting, DateTime dtNow)
        {
            var remainingTime = meeting.StartDateTime - dtNow;
            if (remainingTime.Minutes == 0)
            {
                /*Console.WriteLine($"ВСТРЕЧА С ID={meeting.Id} НАЧАЛАСЬ. " +
                    $"ВРЕМЯ НАЧАЛА {meeting.StartDateTime}, НАПОМИНАНИЕ ЗА " +
                    $"{meeting.MeetingNotificationTimeInMinutes} МИНУТ");*/
                Console.WriteLine(MeetingInfoFormatter.GetStartOfMeetingString(meeting));
            }
        }
    }
}
