namespace PersonalMeetingsManager.Domain.Services
{
    internal interface IManagerService
    {
        Task Execute(string commandString);

        void ShowHelp();
    }
}
