using PersonalMeetingsManager.DAL.Models;
using PersonalMeetingsManager.DAL.Repository;
using System.Globalization;

namespace PersonalMeetingsManager.Domain.CommandHandlers
{
    internal class AddMeetingCommandHandler
    {
        private readonly IMeetingsRepository _repository;

        internal AddMeetingCommandHandler(IMeetingsRepository repository)
        {
            _repository = repository;
        }

        internal async Task AddMeetingAsync()
        {
            var newMeeting = new Meeting();
            IFormatProvider provider = new CultureInfo("ru-RU");

            Console.Write("Введите тему встречи: ");
            var subject = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(subject))
            {
                Console.WriteLine("Тема встречи не может быть пустой. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.Subject = subject;

            Console.Write("Введите время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
            var startDateTimeString = Console.ReadLine();
            DateTime startDateTime;
            try
            {
                startDateTime = DateTime.Parse(startDateTimeString, provider);
            }
            catch (FormatException)
            {
                Console.WriteLine("Задана некорректная дата начала встречи. Операция создания новой встречи отменена");
                return;
            }
            if (startDateTime <= DateTime.Now)
            {
                Console.WriteLine("Встреча может быть назначена только на будущее время. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.StartDateTime = startDateTime;

            Console.Write("Введите время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
            var endDateTimeString = Console.ReadLine();
            DateTime endDateTime;
            try
            {
                endDateTime = DateTime.Parse(endDateTimeString, provider);
            }
            catch (FormatException)
            {
                Console.WriteLine("Задана некорректная дата окончания встречи. Операция создания новой встречи отменена");
                return;
            }
            if (endDateTime <= newMeeting.StartDateTime)
            {
                Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.EndDateTime = endDateTime;

            Console.Write("Введите время напоминания о встрече в минутах: ");
            if (!int.TryParse(Console.ReadLine(), out int notificationTime))
            {
                Console.WriteLine("Задано некорректное время напоминания о встрече. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.MeetingNotificationTimeInMinutes = notificationTime;

            var isMeetingDatesIntersect = await _repository.CheckIntersectMeetingsDatesAsync(newMeeting);
            if (isMeetingDatesIntersect)
            {
                Console.WriteLine("Время новой встречи пересекается с уже сущестующей встречей. Операция создания новой встречи отменена");
                return;
            }

            var meeting = await _repository.AddMeetingAsync(newMeeting);
            Console.WriteLine("Создана новая встреча:");
            Console.WriteLine(MeetingInfoFormatter.GetMeetingInfoString(meeting));
        }
    }
}
