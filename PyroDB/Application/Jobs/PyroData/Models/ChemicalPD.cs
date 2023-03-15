using HtmlAgilityPack;

namespace PyroDB.Application.Jobs.PyroData.Models
{
    public class ChemicalPD
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? Formula { get; set; }

        public void FromNode(HtmlNode node)
        {
            var titleNode = node.Descendants().FirstOrDefault(c => c.Attributes["id"]?.Value?.Contains("page-title") == true);
            var formulaNode = node.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value?.Contains("field-name-field-symbol") == true)?.Descendants("p")?.FirstOrDefault();
            Name = titleNode?.InnerText;
            Formula = formulaNode?.InnerText;
        }
    }


}
