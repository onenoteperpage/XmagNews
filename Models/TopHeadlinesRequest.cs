using XmagNews.Constants;

namespace XmagNews.Models;

public class TopHeadlinesRequest
{
    public string Q { get; set; } = null!;
    public List<string> Sources = new List<string>();
    public Categories? Category { get; set; }
    public Languages? Language { get; set; }
    public Countries? Country { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; }
}