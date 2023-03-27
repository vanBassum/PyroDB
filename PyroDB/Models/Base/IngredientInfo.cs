using PyroDB.Models.Database;

namespace PyroDB.Models.Base
{
    public class IngredientInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float? Quantity { get; set; }
        public virtual ChemicalInfo? Chemical { get; set; }
    }
}
