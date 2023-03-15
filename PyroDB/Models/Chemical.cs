using System.ComponentModel.DataAnnotations;

namespace PyroDB.Models
{
    public class Chemical : ITrackChanges
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Formula { get; set; }
        public virtual DataSourceInfo? DataSourceInfo { get; set; }
        public virtual ICollection<ChangeTrackerItem> Changes { get; set; } = new List<ChangeTrackerItem>();
    }

}