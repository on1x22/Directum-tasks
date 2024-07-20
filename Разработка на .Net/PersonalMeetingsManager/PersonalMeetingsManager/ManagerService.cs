using PersonalMeetingsManager.DAL.Repository;
using PersonalMeetingsManager.Domain;
using PersonalMeetingsManager.Domain.CommandHandlers;

namespace PersonalMeetingsManager
{
    internal class ManagerService : IManagerService
    {
        private readonly IMeetingsRepository _repository;
        private readonly ILastCommandInfo _lastCommandHandler;
        private readonly IConsoleCommandHandler _consoleCommandHandler;
        
        public ManagerService(IMeetingsRepository repository, ILastCommandInfo lastCommandHandler,
                              IConsoleCommandHandler consoleCommandHandler) 
        {
            _repository = repository;
            _lastCommandHandler = lastCommandHandler;
            _consoleCommandHandler = consoleCommandHandler;
        }

        public async Task Execute(string commandString)
        {
            commandString = commandString.Trim();
            if (string.IsNullOrEmpty(commandString))
            {
                Console.WriteLine("Ошибка. Нельзя вводить пустую строку");
                return;
            }

            var arguments = commandString.Split(' ');
            if (arguments.Length == 1)
            {
                switch (arguments[0])
                {
                    case "Помощь":
                        ShowHelp();
                        break;
                    case "Добавить":
                        var amcHandler = new AddMeetingCommandHandler(_repository, 
                                                                      _lastCommandHandler,
                                                                      _consoleCommandHandler);
                        await amcHandler.AddMeetingAsync();
                        //await AddMeetingAsync();
                        break;
                    case "Выход":
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Ошибка. Введена неверная команда");
                        break;
                }
                return;
            }
            
            if (arguments.Length != 2)
            {
                Console.WriteLine("Ошибка. Введена неверная команда");
                return;
            }

            switch (arguments[0])
            {
                case "Просмотреть":                    
                    var gmcHandler = new GetMeetingsCommandHandler(_repository);
                    await gmcHandler.GetMeetingsByDayAsync(arguments[1], new ConsoleMeetingsInfoWriter());
                    break;
                case "Изменить":                    
                    var umcHandler = new UpdateMeetingCommandHandler(_repository);
                    await umcHandler.UpdateMeetingAsync(arguments[1]);
                    //await UpdateMeetingAsync(arguments[1]);
                    break;
                case "Удалить":                    
                    var dmcHandler = new DeleteMeetingCommandHandler(_repository);
                    await dmcHandler.DeleteMeetingAsync(arguments[1]);
                    //await DeleteMeetingAsync(arguments[1]);
                    break;
                case "Экспорт":
                    gmcHandler = new GetMeetingsCommandHandler(_repository);
                    await gmcHandler.GetMeetingsByDayAsync(arguments[1], new FileMeetingsInfoWriter());
                    break;
                default:
                    Console.WriteLine("Ошибка. Введена неверная команда");
                    break;
            }
        }

        /*private async Task GetMeetingsByDayAsync(string dateString, IMeetingsInfoWriter meetingsInfoWriter)
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
        }*/

        /*private async Task AddMeetingAsync()
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
            if (startDateTime <= DateTime.Now )
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
            if (endDateTime <= newMeeting.StartDateTime )
            {
                Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция создания новой встречи отменена");
                return;
            }
            newMeeting.EndDateTime = endDateTime;

            Console.Write("Введите время напоминания о встрече в минутах: ");
            if(!int.TryParse(Console.ReadLine(), out int notificationTime))
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
        }*/

        /*private async Task UpdateMeetingAsync(string meetingIdString)
        {
            IFormatProvider provider = new CultureInfo("ru-RU");
            if(!int.TryParse(meetingIdString, out int meetingId))
            {
                Console.WriteLine("Задан некорректный номер встречи.Операция изменения встречи отменена");
                return;
            }
                        
            var meetingFromDb = await _repository.GetMeetingByIdAsync(meetingId);
            var meetingForUpdate = new Meeting() { Id = meetingFromDb.Id};
            if (meetingFromDb == null)
            {
                Console.WriteLine($"Встреча с Id={meetingId} не существует");
                return;
            }

            Console.WriteLine("Старое значение: " + meetingFromDb.Subject);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новую тему встречи: ");
            var subject = Console.ReadLine();
            if (!string.IsNullOrEmpty(subject))
                meetingForUpdate.Subject = subject;

            Console.WriteLine("Старое значение: " + meetingFromDb.StartDateTime);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новое время начала встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
            var startDateTimeString = Console.ReadLine();
            if (!string.IsNullOrEmpty(startDateTimeString))
            {
                DateTime startDateTime;
                try
                {
                    startDateTime = DateTime.Parse(startDateTimeString, provider);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Задана некорректная дата начала встречи. Операция изменения встречи отменена");
                    return;
                }
                if (startDateTime <= DateTime.Now)
                {
                    Console.WriteLine("Встреча может быть назначена только на будущее время. Операция изменения встречи отменена");
                    return;
                }
                meetingForUpdate.StartDateTime = startDateTime;
            }

            Console.WriteLine("Старое значение: " + meetingFromDb.EndDateTime);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новое время окончания встречи в формате [ДД.ММ.ГГГГ ЧЧ:ММ]: ");
            var endDateTimeString = Console.ReadLine();
            if (!string.IsNullOrEmpty(endDateTimeString))
            {
                DateTime endDateTime;
                try
                {
                    endDateTime = DateTime.Parse(endDateTimeString, provider);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Задана некорректная дата окончания встречи. Операция изменения встречи отменена");
                    return;
                }
                if (endDateTime <= meetingForUpdate.StartDateTime)
                {
                    Console.WriteLine("Время окончания встречи должно быть больше времени начала встречи. Операция изменения встречи отменена");
                    return;
                }
                meetingForUpdate.EndDateTime = endDateTime;
            }

            Console.WriteLine("Старое значение: " + meetingFromDb.MeetingNotificationTimeInMinutes);
            Console.WriteLine("Для сохранения старого значения оставьте пустую строку и нажмите Enter");
            Console.Write("Введите новое время напоминания о встрече в минутах: ");
            var notificationTimeString = Console.ReadLine();
            if (!string.IsNullOrEmpty(notificationTimeString))
            {
                if (!int.TryParse(notificationTimeString, out int notificationTime))
                {
                    Console.WriteLine("Задано некорректное время напоминания о встрече. Операция изменения встречи отменена");
                    return;
                }

                meetingForUpdate.MeetingNotificationTimeInMinutes = notificationTime;
            }

            var isMeetingDatesIntersect = await _repository.CheckIntersectMeetingsDatesAsync(meetingForUpdate);
            if (isMeetingDatesIntersect)
            {
                Console.WriteLine("Время измененной встречи пересекается с уже сущестующей встречей. Операция создания новой встречи отменена");
                return;
            }

            var updatedMeeting = await _repository.UpdateMeetingAsync(meetingForUpdate);
            Console.WriteLine("Встреча изменена:");
            Console.WriteLine(MeetingInfoFormatter.GetMeetingInfoString(updatedMeeting));
        }*/

        /*private async Task DeleteMeetingAsync(string meetingIdString)
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
        }*/

        public void ShowHelp()
        {
            Console.WriteLine("Для работы с менеждером встреч введите одну из команд:");
            Console.WriteLine("\tПросмотреть [дата]\tПросмотр информации о встречах за указанный день в формате ДД.ММ.ГГГГ");
            Console.WriteLine("\tДобавить\t\tДобавление новой встречи");
            Console.WriteLine("\tИзменить [Id встречи]\tИзменениие информации о встрече");
            Console.WriteLine("\tУдалить [Id встречи]\tУдаление встречи из списка");
            Console.WriteLine("\tЭкспорт [дата]\t\tЭкспортировать в текстовый файл список встреч за указанный день в формате ДД.ММ.ГГГГ");
            Console.WriteLine("\tПомощь\t\t\tОтображение списка доступных команд");
            Console.WriteLine("\tВыход\t\t\tЗакрыть приложение");
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }

        /*private static DateTime CheckDateFormat(string dateString)
        {
            IFormatProvider provider = new CultureInfo("ru-RU");
            var date = DateTime.Parse(dateString, provider);
            return date;
        }*/
    }
}
