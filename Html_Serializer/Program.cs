using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Html_Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
 
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var html = await Load("https://yourweddingcountdown.com/4fcbd");
        //div.wrapper .container p
        // .one h6 a
        //div .clear
        var cleanhtml = new Regex("\\s").Replace(html," ");  
        var htmlLines = new Regex("<(.*?)>").Split(cleanhtml).Where(s => s.Length > 0).ToList();  
        htmlLines = htmlLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
        HtmlElement root = HtmlElement.htmlElementsfunc(htmlLines);
        PrintTree(root, " ");
        Console.ReadLine();
        Selector select1 = Selector.ParseQueryString("div .clear");
        var result =root.ElementsBySelector(select1);
        var mul=new HashSet<HtmlElement>(result);
         foreach ( var item in mul)
        {
            Console.WriteLine(item.Name);
        }
         return 0;
    } 
    public static async Task<string> Load(string url)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        return html;
    }
    static void PrintTree(HtmlElement root, string indent)
    {
        Console.WriteLine();

        Console.WriteLine(indent + "Id: " + root.Id);

        Console.WriteLine(indent + "Tag name: " + root.Name);

        Console.WriteLine(indent + "Attributes:");
        if (root.Attributes != null)
        {
            foreach (var attribute in root.Attributes)
            {
                Console.WriteLine(indent + "* " + attribute);
            }
        }
        Console.WriteLine(indent + "Classes:");
        if (root.Classes != null)
        {
            foreach (var className in root.Classes)
            {
                Console.WriteLine(indent + "* " + className);
            }
        }

        Console.WriteLine(indent + "Inner HTML: " + root.InnerHtml);

        Console.WriteLine(indent + "Parent: " + (root.Parent != null ? root.Parent.Name : "None"));

        Console.WriteLine(indent + "Children:");
        foreach (var child in root.Children)
        {
            PrintTree(child, indent + "  ");
        }
    }   
    

}

