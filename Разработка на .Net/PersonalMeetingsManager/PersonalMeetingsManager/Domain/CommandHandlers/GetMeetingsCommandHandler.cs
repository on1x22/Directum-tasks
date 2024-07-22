using PersonalMeetingsManager.DAL.Repository;
using PersonalMeetingsManager.Infrastructure;
using System.Globalization;

namespace PersonalMeetingsManager.Domain.CommandHandlers
{
    internal class GetMeetingsCommandHandler
    {
        private readonly IMeetingsRepository _repository;
        internal GetMeetingsCommandHandler(IMeetingsRepository repository)
        {
            _repository = repository;
        }
        internal async Task GetMeetingsByDayAsync(string dateString, IMeetingsInfoWriter meetingsInfoWriter)
        {
            DateTime date;
            try
            {
                date = CheckDateFormat(dateString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка. Задан некорректный формат даты");
                return;
            }

            var meetings = await _repository.GetMeetingsByDayAsync(date);
            if (meetings.Count == 0)
            {
                Console.WriteLine($"Встречи за {date:dd/MM/yyyy} отсутствуют");
                return;
            }

            await meetingsInfoWriter.Write(meetings);
        }

        private static DateTime CheckDateFormat(string dateString)
        {
            IFormatProvider provider = new CultureInfo("ru-RU");
            var date = DateTime.Parse(dateString, provider);
            return date;
        }
    }
}
