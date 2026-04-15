namespace SAFORIX.Models
{
    public class HomeContent
    {
        public int Id { get; set; }
        public string SectionKey { get; set; } = "";
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
