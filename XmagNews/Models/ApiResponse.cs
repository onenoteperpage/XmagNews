using XmagNews.Constants;

namespace XmagNews.Models;

internal class ApiResponse
{
    public Statuses Status { get; set; }
    public ErrorCodes? Code { get; set; }
    public string? Message { get; set; }
    public List<Article>? Articles { get; set; }
    public int TotalResults { get; set; }
}