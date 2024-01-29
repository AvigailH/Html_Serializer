using Html_Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
public class Selector
{
    public string TagName { get; set; }
    public string Id { get; set; }
    public List<string> Classes { get; set; }
    public Selector Parent { get; set; }
    public Selector Child { get; set; }

    public Selector()
    {
        TagName = "";
        Id = "";
        Classes = new List<string>();
        Parent = null;
        Child = null;
    }
    public static Selector ParseQueryString(string queryString)
    {
        var htmlHelper = HtmlHelper.Instance;

        Selector newSelector;
        Selector root = new Selector();
        Selector currentSelector = root;

        if (string.IsNullOrEmpty(queryString))
        {
            return null;
        }
        string[] selectors = queryString.Split(' ');
        foreach (string selector in selectors)
        {
            newSelector = new Selector();
            newSelector.Parent = currentSelector;
            currentSelector.Child = newSelector;
            string[] parts = selector.Split('#', '.');
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if ((selector.IndexOf(part) - 1) >= 0)
                {
                    char prevChar = selector[selector.IndexOf(part) - 1];

                    if (prevChar == '#')
                    {
                        currentSelector.Id = part;
                    }
                    else if (prevChar == '.')
                    {
                        currentSelector.Classes.Add(part);
                    }
                    else if (prevChar == ' ')
                    {
                        currentSelector.TagName = part;
                    }
                }
                else
                {
                    currentSelector.TagName = part;
                }
            }
            currentSelector = newSelector;
        }

        return root;
    }

    public static void PrintTree(Selector root)
        {
            Selector current = root;
            while (current != null)
            {
                Console.WriteLine("TagName: " + current.TagName);
                Console.WriteLine("Id: " + current.Id);
                Console.WriteLine("Classes:");
                foreach (var className in current.Classes)
                {
                    Console.WriteLine("- " + className);
                }
                Console.WriteLine("Parent: " + (current.Parent != null ? current.Parent.TagName : "None"));
                Console.WriteLine("Child: " + (current.Child != null ? current.Child.TagName : "None"));

                current = current.Child;
            }
        }
    public override bool Equals(object? obj)
    {
        if (obj is HtmlElement)
        {
            HtmlElement? element = obj as HtmlElement;


            if (element != null)
            {

                bool tagNameMatches = string.IsNullOrEmpty(TagName) || TagName == element.Name;
                bool idMatches = string.IsNullOrEmpty(Id) || Id == element.Id;
                bool classesMatch = Classes.All(className => element.Classes.Contains(className));

                return tagNameMatches && idMatches && classesMatch;
            }

        }
        return false;
    }
}