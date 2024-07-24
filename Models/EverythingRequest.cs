using XmagNews.Constants;

namespace XmagNews.Models;

public class EverythingRequest
    {
        public string Q { get; set; } = null!;
        public List<string> Sources = new List<string>();
        public List<string> Domains = new List<string>();
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Languages? Language { get; set; }
        public SortBys? SortBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }