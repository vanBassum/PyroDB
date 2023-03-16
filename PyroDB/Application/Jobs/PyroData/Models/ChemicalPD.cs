using HtmlAgilityPack;
using PyroDB.Application.Extentions;

namespace PyroDB.Application.Jobs.PyroData.Models
{
    public class ChemicalPD
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? Formula { get; set; }
        public void FromNode(HtmlNode node)
        {
            Name = node.Query("#page-title")?.InnerText;
            Formula = node.Query("div .field-name-field-symbol .field-item")?.InnerText;

        }
    }


}
