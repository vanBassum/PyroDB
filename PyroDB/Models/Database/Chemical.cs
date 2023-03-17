using System.ComponentModel.DataAnnotations;

namespace PyroDB.Models.Database
{
    public class Chemical : ITrackChanges
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Formula { get; set; }
        public virtual DataSourceInfo? DataSourceInfo { get; set; }
        public virtual ICollection<ChangeTrackerItem> Changes { get; set; } = new List<ChangeTrackerItem>();
        public virtual ICollection<ApplicationUser> OwnedBy { get; set; } = new List<ApplicationUser>();
    }
}