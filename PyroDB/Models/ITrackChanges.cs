namespace PyroDB.Models
{
    public interface ITrackChanges
    {
        public ICollection<ChangeTrackerItem> Changes { get; set; }
    }
}
