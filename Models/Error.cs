using XmagNews.Constants;

namespace XmagNews.Models;

public class Error
{
    public ErrorCodes Code { get; set; }
    public string? Message { get; set; }
}