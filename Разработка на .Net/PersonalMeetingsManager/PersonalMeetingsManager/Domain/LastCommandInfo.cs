namespace PersonalMeetingsManager.Domain
{
    internal class LastCommandInfo : ILastCommandInfo
    {       
        public string LastCommand { get; set; }
        public int LastPosition { get; set; }

    }
}
