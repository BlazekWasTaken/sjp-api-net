namespace sjp_api_net;

public static class Sjp
{
    
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