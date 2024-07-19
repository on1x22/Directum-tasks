using PersonalMeetingsManager.DAL.Models;

namespace PersonalMeetingsManager.Domain
{
    public class FileMeetingsInfoWriter : IMeetingsInfoWriter
    {
        public async Task Write(List<Meeting> meetings)
        {
            string directory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(directory, "MeetingsExport.txt");

            await using var sw = new StreamWriter(filePath);
            foreach (var meeting in meetings)
            {
                await sw.WriteLineAsync(MeetingInfoFormatter.GetMeetingInfoString(meeting));
            }

            Console.WriteLine("Данные успешно записаны в файл " + filePath);
        }
    }
}
