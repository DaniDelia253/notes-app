namespace API.Models
{
    public class Note
    {
        public int note_id { get; set; }
        public int user_id { get; set; }
        public string? title { get; set; }
        public string? content { get; set; }

    }
}