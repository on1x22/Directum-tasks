namespace PersonalMeetingsManager.Domain
{
    internal interface ILastCommandInfo
    {
        string LastCommand { get; set; }
        int LastPosition { get; set; }
    }
}
