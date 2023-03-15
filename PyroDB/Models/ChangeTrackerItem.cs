using System.ComponentModel.DataAnnotations;

namespace PyroDB.Models
{
    public class ChangeTrackerItem
    {
        [Key]
        public int Id { get; set; }
        public DataSources Source { get; set; }
        public ChangeTrackerTypes ChangeTrackerType { get; set; }
        public DateTime? Timestamp { get; set; }
        public ChangeTrackerItem() { }

        public ChangeTrackerItem(DataSources source, ChangeTrackerTypes changeTrackerType)
        {
            Source = source;
            ChangeTrackerType = changeTrackerType;
            Timestamp = DateTime.Now;
        }
    }


}
