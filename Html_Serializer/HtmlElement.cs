using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
 
public class HtmlElement
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Attributes { get; set; }
    public List<string> Classes { get; set; }
    public string InnerHtml { get; set; }
    public HtmlElement Parent { get; set; }
    public List<HtmlElement> Children { get; set; }

    public HtmlElement()
    {
        Id = "";
        Name = "";
        Attributes = new List<string>();
        Classes = new List<string>();
        InnerHtml = string.Empty;
        Parent = null;
        Children = new List<HtmlElement>();
    }
     
  public IEnumerable<HtmlElement> Descendants()
{
        Queue<HtmlElement> queue = new Queue<HtmlElement>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            HtmlElement currentHtmlElement = queue.Dequeue();
            yield return currentHtmlElement;

            foreach (HtmlElement child in currentHtmlElement.Children ?? Enumerable.Empty<HtmlElement>())
            {
                queue.Enqueue(child);
            }
        }
    }
  public IEnumerable<HtmlElement> Ancestors(HtmlElement element)
    {
        HtmlElement currentHtmlElement = element.Parent;

        while (currentHtmlElement != null)
        {
            yield return currentHtmlElement;
            currentHtmlElement = currentHtmlElement.Parent;
        }
    }
   

    public static HtmlElement htmlElementsfunc(IEnumerable<string> htmlLines)
    {
        HtmlElement newElement;
        HtmlElement root = new HtmlElement();
        HtmlElement current = root;
        List<string> listClass = new List<string>();
        List<string> listAttributes = new List<string>();
        foreach (string line in htmlLines)
        {
            string tag = line.Split(' ')[0];

            if (tag == "/html")
            {
                return root;
            }
            else if (tag.StartsWith("/"))
            {
                if (current.Parent != null)
                {
                    current = current.Parent;
                }
            }
            else if (HtmlHelper.Instance.AllTags.Contains(tag))
            {
                var attributees = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
                newElement = new HtmlElement();
                newElement.Name = tag;
                current.Children.Add(newElement);
                newElement.Parent = current;
                foreach (Match attribute in attributees)
                {
                    var attributeName = attribute.Groups[1].Value;
                    var attributeValue = attribute.Groups[2].Value;
                    switch (attributeName)
                    {
                        case "class":
                            {
                                listClass = attributeValue.Split(' ').ToList();
                                foreach (string s in listClass)
                                {
                                    newElement.Classes.Add(s);
                                }
                            }
                            break;

                        case "id":
                            {
                                var id = attributeValue;
                                newElement.Id = attributeValue;
                            }
                            break;
                        default:
                            {
                                listAttributes = attributeValue.Split(' ').ToList();
                                foreach (string la in listAttributes)
                                {
                                    newElement.Attributes.Add(la);
                                }
                            }
                            break;
                    }
                }
                if (!HtmlHelper.Instance.HtmlVoidTagsone.Contains(tag) || !(line[line.Length - 1] == '/'))
                {
                    current = newElement;
                }
            }
            else
            {
                current.InnerHtml = line;
            }
        }
        return root;
    }
 
}



