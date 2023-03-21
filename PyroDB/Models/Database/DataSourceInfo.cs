using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PyroDB.Models.Database
{
    public class DataSourceInfo
    {
        [Key]
        public int Id { get; set; }
        public DataSources DataSource { get; set; }
        public string? SourceId { get; set; }

        public override string ToString() => DataSource.ToString();
    }


}
