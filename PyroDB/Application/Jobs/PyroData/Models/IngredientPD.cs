using HtmlAgilityPack;

namespace PyroDB.Application.Jobs.PyroData.Models
{
    public class IngredientPD
    {
        public string? Name { get; set; }
        public string? Quantity { get; set; }
        public string? ChemicalLink { get; set; }
        public void FromNode(HtmlNode node)
        {
            var nameNode = node.Descendants("span").Where(a => a.Attributes["class"].Value.Contains("ingredient-name")).FirstOrDefault();
            var quantityNode = node.Descendants("div").Where(a => a.Attributes["class"].Value.Contains("quantity-unit")).FirstOrDefault();
            var linkNode = nameNode?.Descendants("a").FirstOrDefault();
            Name = nameNode?.InnerText;
            Quantity = quantityNode?.InnerText;
            ChemicalLink = linkNode?.Attributes["href"]?.Value;
        }
    }
}
