using HtmlAgilityPack;

namespace PyroDB.Application.Synchronizers.PyroData
{
    public class RecipePD
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public List<IngredientPD> Ingredients { get; set; } = new List<IngredientPD>();

        public void FromNode(HtmlNode node)
        {
            var titleNode = node.Descendants().FirstOrDefault(c => c.Attributes["id"]?.Value?.Contains("page-title") == true);
            Name = titleNode?.InnerText;
            FindIngredients(node);
        }

        private void FindIngredients(HtmlNode rootNode)
        {
            var nodes = rootNode.Descendants("div").Where(a => a.Attributes["typeof"]?.Value?.Contains("schema:Ingredient") == true);
            foreach (var node in nodes)
            {
                IngredientPD ingredient = new IngredientPD();
                ingredient.FromNode(node);
                Ingredients.Add(ingredient);
            }
        }

    }
}
