using PersonalMeetingsManager.DAL.Repository;
using PersonalMeetingsManager.Domain.CommandHandlers;
using PersonalMeetingsManager.Infrastructure;

namespace PersonalMeetingsManager.Domain.Services
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
            if (!CheckEmptyString(commandString))
                return;

            var arguments = commandString.Split(' ');
            if (!await CheckCommandWithoutParameters(arguments))
                return;

            if (arguments.Length != 2)
            {
                Console.WriteLine("Ошибка. Введена неверная команда");
                return;
            }

            if (!await CheckCommandWithOneParameter(arguments))
                return;
        }

        private bool CheckEmptyString(string commandString)
        {
            if (string.IsNullOrEmpty(commandString))
            {
                Console.WriteLine("Ошибка. Нельзя вводить пустую строку");
                return false;
            }
            return true;
        }

        private async Task<bool> CheckCommandWithoutParameters(string[] arguments)
        {
            if (arguments.Length == 1)
            {
                switch (arguments[0])
                {
                    case "Помощь":
                        ShowHelp();
                        return false;
                    case "Добавить":
                        var amcHandler = new AddMeetingCommandHandler(_repository,
                                                                      _lastCommandHandler,
                                                                      _consoleCommandHandler);
                        await amcHandler.AddMeetingAsync();
                        return false;
                    case "Выход":
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Ошибка. Введена неверная команда");
                        return false;
                }
                return true;
            }
            return true;
        }

        private async Task<bool> CheckCommandWithOneParameter(string[] arguments)
        {
            switch (arguments[0])
            {
                case "Просмотреть":
                    var gmcHandler = new GetMeetingsCommandHandler(_repository);
                    await gmcHandler.GetMeetingsByDayAsync(arguments[1], new ConsoleMeetingsInfoWriter());
                    break;
                case "Изменить":
                    var umcHandler = new UpdateMeetingCommandHandler(_repository, _consoleCommandHandler);
                    await umcHandler.UpdateMeetingAsync(arguments[1]);
                    break;
                case "Удалить":
                    var dmcHandler = new DeleteMeetingCommandHandler(_repository);
                    await dmcHandler.DeleteMeetingAsync(arguments[1]);
                    break;
                case "Экспорт":
                    gmcHandler = new GetMeetingsCommandHandler(_repository);
                    await gmcHandler.GetMeetingsByDayAsync(arguments[1], new FileMeetingsInfoWriter());
                    break;
                default:
                    Console.WriteLine("Ошибка. Введена неверная команда");
                    return false;
            }
            return true;
        }

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
    }
}
