namespace SAFORIX.Models
{
    public class HomeFeature
    {
        public int Id { get; set; }
        public string? Icon { get; set; }       // e.g. "lightning", "shield", "brain"
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
