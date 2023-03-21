using PyroDB.Models.Database;

namespace PyroDB.Models.Base
{
    public class IngredientInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float? Quantity { get; set; }
        public virtual ChemicalInfo? Chemical { get; set; }

        public static IngredientInfo Create(Ingredient dbItem)
        {
            IngredientInfo result = new IngredientInfo
            {
                Id = dbItem.Id,
                Name = dbItem.Name,
                Quantity = dbItem.Quantity,
            };

            return result;
        }
    }
}
