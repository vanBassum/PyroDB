using System.ComponentModel.DataAnnotations;

namespace PyroDB.Models.Database
{
    public class Recipe : ITrackChanges
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Instructions { get; set; }
        public string? Source { get; set; }
        public string? Video { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public virtual DataSourceInfo? DataSourceInfo { get; set; }
        public virtual ICollection<ChangeTrackerItem> Changes { get; set; } = new List<ChangeTrackerItem>();
    }
}
