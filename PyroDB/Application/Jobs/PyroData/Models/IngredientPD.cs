using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PyroDB.Application.Extentions;

namespace PyroDB.Application.Jobs.PyroData.Models
{
    public class IngredientPD
    {
        public string? Name { get; set; }
        public string? Quantity { get; set; }
        public string? ChemicalLink { get; set; }
        public void FromNode(HtmlNode node)
        {
            Name = node.Query(".ingredient-name")?.InnerText;
            Quantity = node.Query(".quantity-unit")?.InnerText;
            ChemicalLink = node.Query(".ingredient-name a")?.Attributes["href"]?.Value;
        }
    }
}
