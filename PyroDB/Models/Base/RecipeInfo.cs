using Microsoft.EntityFrameworkCore;
using PyroDB.Models.Database;

namespace PyroDB.Models.Base
{
    public class RecipeInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<IngredientInfo> Ingredients { get; set; } = new List<IngredientInfo>();
    }
}
