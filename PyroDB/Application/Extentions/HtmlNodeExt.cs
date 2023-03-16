using HtmlAgilityPack;

namespace PyroDB.Application.Extentions
{
    public static class HtmlNodeExt
    {
        public static HtmlNode? Query(this HtmlNode node, string query)
        {
            var split = query.Split(' ');
            IEnumerable<HtmlNode> nodes = new List<HtmlNode> { node };
            foreach (var v in split)
            {
                string search = v.TrimStart('.', '#');

                if (v.StartsWith('.'))
                {
                    nodes = nodes.SelectMany(a => a.Descendants()).Where(n=>n.HasClass(search));
                }
                else if (v.StartsWith('#'))
                {
                    nodes = nodes.SelectMany(a => a.Descendants()).Where(n => n.Id == search);
                }
                else
                {
                    nodes = nodes.SelectMany(a => a.Descendants(search));
                }
            }
            return nodes.FirstOrDefault();
        }
    }
}
