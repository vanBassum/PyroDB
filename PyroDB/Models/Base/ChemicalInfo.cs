using PyroDB.Models.Database;

namespace PyroDB.Models.Base
{
    public class ChemicalInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Formula { get; set; }
        public bool Owned { get; set; } = false;


        public static ChemicalInfo Create(Chemical dbItem)
        {
            ChemicalInfo result = new ChemicalInfo
            {
                Id= dbItem.Id,
                Name= dbItem.Name,
                Formula= dbItem.Formula,
            };
            return result;
        }
    }
}
