using System.Diagnostics;
using Newtonsoft.Json;
using XmagNews.Constants;
using XmagNews.Models;

namespace XmagNews;

public class XmagNews
{
    private string BASE_URL = "https://newsapi.org/v2/";
    
    private string _apiKey;

    private HttpClient httpClient;

    public XmagNews(string apiKey)
    {
        _apiKey = apiKey;

        httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate });
        httpClient.DefaultRequestHeaders.Add("user-agent", "XmagNews-Agent-csharp/0.1");
        httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
    }

    public async Task<ArticlesResult> GetTopHeadlinesAsync(TopHeadlinesRequest request)
    {
        // build query string
        var queryParams = new List<string>();

        // q
        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            queryParams.Add("q=" + request.Q);
        }

        // sources
        if (request.Sources.Count > 0)
        {
            queryParams.Add("sources=" + string.Join(",", request.Sources));
        }

        // category
        if (request.Category.HasValue)
        {
            queryParams.Add("category=" + request.Category.Value.ToString().ToLowerInvariant());
        }

        // language
        if (request.Language.HasValue)
        {
            queryParams.Add("language" + request.Language.Value.ToString().ToLowerInvariant());
        }

        // country
        if (request.Country.HasValue)
        {
            queryParams.Add("country=" + request.Country.Value.ToString().ToLowerInvariant());
        }

        // page
        if (request.Page > 1)
        {
            queryParams.Add("page" + request.Page);
        }

        // page size
        if (request.PageSize > 0)
        {
            queryParams.Add("pageSize" + request.PageSize);
        }

        // join string
        var queryString = string.Join("&", queryParams.ToArray());

        return await MakeRequest("top-headlines", queryString);
    }

    public async Task<ArticlesResult> MakeRequest(string endpoint, string querystring)
{
    // return obj
    var articlesResult = new ArticlesResult();

    // make http request
    var httpRequest = new HttpRequestMessage(HttpMethod.Get, BASE_URL + endpoint + "?" + querystring);
    var httpResponse = await httpClient.SendAsync(httpRequest);

    var httpContent = httpResponse.Content;
    if (httpContent != null)
    {
        var json = await httpContent.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(json))
        {
            // convert json to obj
            ApiResponse? apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);

            if (apiResponse != null)
            {
                articlesResult.Status = apiResponse.Status;
                if (articlesResult.Status == Statuses.Ok)
                {
                    articlesResult.TotalResults = apiResponse.TotalResults;
                    articlesResult.Articles = apiResponse.Articles;
                }
                else
                {
                    ErrorCodes errorCode = ErrorCodes.UnexpectedError;
                    try
                    {
                        if (apiResponse.Code.HasValue)
                        {
                            errorCode = (ErrorCodes)apiResponse.Code.Value;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("The API returned an error code that wasn't expected: " + apiResponse.Code);
                        Debug.WriteLine($"Error Msg: {e}");
                    }

                    articlesResult.Error = new Error
                    {
                        Code = errorCode,
                        Message = apiResponse.Message,
                    };
                }
            }
        }
    }
    else
    {
        articlesResult.Status = Statuses.Error;
        articlesResult.Error = new Error
        {
            Code = ErrorCodes.UnexpectedError,
            Message = "The API returned an empty response. Is internet connection enabled?",
        };
    }

    return articlesResult;
}

}
