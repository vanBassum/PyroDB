using PyroDB.Models.Database;

namespace PyroDB.Models.Base
{
    public class RecipeInfo
    {
        public Recipe DbRecipe { get; set; }
        public bool HasIngredients { get; set; } = false;


        public RecipeInfo(Recipe dbRecipe, bool hasIngredients)
        {
            DbRecipe = dbRecipe;
            HasIngredients = hasIngredients;
        }


    }
}
