using PyroDB.Models.Database;

namespace PyroDB.Models.Base
{
    public class RecipeInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<IngredientInfo> Ingredients { get; set; } = new List<IngredientInfo>();

        public static RecipeInfo Create(Recipe dbItem)
        {
            RecipeInfo result = new RecipeInfo
            {
                Id = dbItem.Id,
                Name =dbItem.Name
            };
            return result;
        }
    }
}
