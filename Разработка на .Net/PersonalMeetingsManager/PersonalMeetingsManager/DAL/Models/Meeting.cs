using System.ComponentModel.DataAnnotations;

namespace PersonalMeetingsManager.DAL.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MeetingNotificationTimeInMinutes { get; set; }

        public Meeting Clone() =>        
            new Meeting { 
                Id = Id, 
                Subject = Subject, 
                StartDateTime = StartDateTime, 
                EndDateTime = EndDateTime 
            }; 
        
    }
}
