using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Html_Serializer
{
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> ElementsBySelector(this HtmlElement element, Selector selector)
        {
            if (element == null || selector == null)
                yield break;

            var descendants = element.Descendants();

            foreach (var descendant in descendants)
            {
                if (selector.Equals(descendant))
                {
                    if (selector.Child != null)
                    {
                        var newList = descendant.ElementsBySelector(selector.Child);
                        foreach (var item in newList)
                        {
                            yield return item;
                        }
                    }
                    else
                    {
                        descendant.Children = null;
                        yield return descendant; 
                    }

                 }
            }
            yield break;

        }
    }
}
