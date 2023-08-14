using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace sjp_api_net;

public static class Sjp
{
    public static Word GetWord(string word)
    {
        word = word.ToLower().Trim();
        string url = $"https://sjp.pl/{word}";

        string result;
        try
        {
            HttpResponseMessage response = new HttpClient().GetAsync(url).Result;
            HttpContent content = response.Content;
            result = content.ReadAsStringAsync().Result;
        }
        catch
        {
            throw new Exception();
        }
        
        if (result.Contains("nie występuje w słowniku")) throw new Exception();
        
        IHtmlDocument document = new HtmlParser().ParseDocument(result);

        if (document.QuerySelectorAll("h1").All(x => x.InnerHtml.ToLower().Trim() != word)) 
            throw new Exception();

        string isAllowedHtml = document.QuerySelectorAll("p")
            .FirstOrDefault(x => x.InnerHtml.Contains("dopuszczalne"))?
            .InnerHtml ?? string.Empty;

        if (!isAllowedHtml.Contains("dopuszczalne")) throw new Exception();

        string isAllowed = isAllowedHtml.Split(" ").FirstOrDefault(x => x.Contains("dopuszczalne")) 
                           ?? string.Empty;

        List<string> definitions = document.QuerySelectorAll("p")[3].InnerHtml.Split("<br>").ToList();

        return new Word(word, isAllowed == "dopuszczalne", definitions);
    }
}

public class Word
{
    private string Name { get; set; }
    private bool IsAllowedInGames { get; set; }
    private List<string> Definitions { get; set; }
    public Word(string name, bool isAllowedInGames, List<string> definitions)
    {
        Name = name;
        IsAllowedInGames = isAllowedInGames;
        Definitions = definitions;
    }
    public string GetName()
    {
        return Name;
    }
    public bool GetIsAllowedInGames()
    {
        return IsAllowedInGames;
    }
    public List<string> GetDefinitions()
    {
        return Definitions;
    }
}