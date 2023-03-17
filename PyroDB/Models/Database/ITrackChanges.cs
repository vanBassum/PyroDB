namespace PyroDB.Models.Database
{
    public interface ITrackChanges
    {
        public ICollection<ChangeTrackerItem> Changes { get; set; }
    }
}
