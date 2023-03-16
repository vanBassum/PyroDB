using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PyroDB.Application.Extentions;

namespace PyroDB.Application.Jobs.PyroData.Models
{
    public class RecipePD
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Instructions { get; set; }
        public string? Source { get; set; }
        public string? Video { get; set; }
        public List<IngredientPD> Ingredients { get; set; } = new List<IngredientPD>();

        public void FromNode(HtmlNode node)
        {
            Name = node.Query("#page-title")?.InnerText;
            Description = node.Query(".recipe-description .section")?.InnerText;
            Instructions = node.Query(".recipe-instruction .section")?.InnerText;
            Source = node.Query(".field-name-field-source .field-item")?.InnerText;
            Video = node.Query(".player iframe")?.Attributes["src"]?.Value;
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
