namespace PersonalMeetingsManager.Infrastructure
{
    internal interface ILastCommandInfo
    {
        string LastCommand { get; set; }
        int LastPosition { get; set; }
    }
}
