namespace SAFORIX.Models
{
    public class HomeHero
    {
        public int Id { get; set; }
        public string MainTitle { get; set; } = "";
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonLink { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
