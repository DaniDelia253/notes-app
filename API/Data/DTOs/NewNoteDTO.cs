namespace API.Data.DTOs
{
    public class NewNoteDTO
    {
        public int user_id { get; set; }
        public string? title { get; set; }
        public string? content { get; set; }
    }
}