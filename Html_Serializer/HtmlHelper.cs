using System;
using System.IO;
using System.Text.Json;

public class HtmlHelper
{
    private readonly static HtmlHelper _instance = new HtmlHelper();
    public static HtmlHelper Instance => _instance;
    public string[] AllTags { get; private set; }
    public string[] HtmlVoidTagsone { get; private set; }

    private HtmlHelper()
    {
        try
        {
            string allTagsJson = File.ReadAllText("HtmlTags.json");
            string HtmlVoidTags = File.ReadAllText("HtmlVoidTags.json");
            AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
            HtmlVoidTagsone = JsonSerializer.Deserialize<string[]>(HtmlVoidTags);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading HTML files: {ex.Message}");
         }
    }

}
