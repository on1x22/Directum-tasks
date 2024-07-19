using PersonalMeetingsManager.DAL.Repository;

namespace PersonalMeetingsManager.Domain.CommandHandlers
{
    internal class DeleteMeetingCommandHandler
    {
        private readonly IMeetingsRepository _repository;

        internal DeleteMeetingCommandHandler(IMeetingsRepository repository)
        {
            _repository = repository;
        }

        internal async Task DeleteMeetingAsync(string meetingIdString)
        {
            if (!int.TryParse(meetingIdString, out int meetingId))
            {
                Console.WriteLine($"Ошибка. Введен некорректный Id встречи");
                return;
            }

            var meeting = await _repository.DeleteMeetingAsync(meetingId);
            if (meeting == null)
            {
                Console.WriteLine($"Встреча с Id={meetingId} не существует");
                return;
            }

            Console.WriteLine($"Встреча с Id={meetingId} удалена");
            return;
        }
    }
}
