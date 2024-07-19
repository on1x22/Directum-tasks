namespace PersonalMeetingsManager
{
    internal interface IManagerService
    {
        Task Execute(string commandString);

        void ShowHelp();
    }
}
