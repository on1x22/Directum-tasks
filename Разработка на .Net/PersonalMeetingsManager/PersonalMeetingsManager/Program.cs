using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalMeetingsManager.DAL.Contexts;
using PersonalMeetingsManager.DAL.Repository;
using PersonalMeetingsManager.Domain;

namespace PersonalMeetingsManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IMeetingsRepository, MeetingsRepository>()
                .AddSingleton<IManagerService, ManagerService>()
                .AddSingleton<IMeetingsInspectorService, MeetingsInspectorService>()
                .AddDbContext<MeetingsDbContext>(options =>
                    options.UseInMemoryDatabase("MeetingsDb"))
                .BuildServiceProvider();
            

            Console.WriteLine("ПРИЛОЖЕНИЕ ДЛЯ УПРАВЛЕНИЯ ЛИЧНЫМИ ВСТРЕЧАМИ");
            var ms = serviceProvider.GetService<IManagerService>();
            ms.ShowHelp();
            Console.WriteLine();

            var mi = serviceProvider.GetService<IMeetingsInspectorService>();
            //Thread inspectNotifications = new Thread(mi.InspectMeetings);
            //inspectNotifications.Start();
            await Task.Run(mi.InspectMeetings);

            while (true)
            {                
                Console.Write("Введите команду: ");
                var command = Console.ReadLine();                
                await ms.Execute(command);
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
