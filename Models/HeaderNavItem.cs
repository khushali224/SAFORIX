namespace SAFORIX.Models
{
    /// <summary>
    /// Dynamic header navigation - loaded from backend
    /// ParentId = null for top level, set for dropdown children (e.g. Services submenu)
    /// </summary>
    public class HeaderNavItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; } = "";
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public int? ParentId { get; set; }    // null = top nav, non-null = dropdown child
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
