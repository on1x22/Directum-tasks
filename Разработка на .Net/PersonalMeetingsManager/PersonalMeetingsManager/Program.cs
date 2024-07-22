using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalMeetingsManager.DAL.Contexts;
using PersonalMeetingsManager.DAL.Repository;
using PersonalMeetingsManager.Domain.Services;
using PersonalMeetingsManager.Infrastructure;

var serviceProvider = new ServiceCollection()
                .AddSingleton<IMeetingsRepository, MeetingsRepository>()
                .AddSingleton<IManagerService, ManagerService>()
                .AddSingleton<IMeetingsInspectorService, MeetingsInspectorService>()
                .AddSingleton<ILastCommandInfo, LastCommandInfo>()
                .AddTransient<IConsoleCommandHandler, ConsoleCommandHandler>()
                .AddDbContext<MeetingsDbContext>(options =>
                    options.UseInMemoryDatabase("MeetingsDb"))
                .BuildServiceProvider();


Console.WriteLine("ПРИЛОЖЕНИЕ ДЛЯ УПРАВЛЕНИЯ ЛИЧНЫМИ ВСТРЕЧАМИ");
var ms = serviceProvider.GetService<IManagerService>();
ms.ShowHelp();
Console.WriteLine();

var mi = serviceProvider.GetService<IMeetingsInspectorService>();
Task ds = Task.Run(mi.InspectMeetingsAsync);

var lastCommandHandler = serviceProvider.GetService<ILastCommandInfo>();

while (true)
{
    var lastCommand = "Введите команду: ";
    lastCommandHandler.LastCommand = lastCommand;
    Console.Write(lastCommand);
    var command = Console.ReadLine();
    await ms.Execute(command);
    Console.WriteLine();
}
