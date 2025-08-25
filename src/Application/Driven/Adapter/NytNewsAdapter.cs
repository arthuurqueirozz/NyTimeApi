using System.Text.Json;
using Core.Entities;
using Core.Ports.Services.NytNews;

namespace Application.Driven.Adapter
{
    public class NytNewsAdapter : INytNewsPort
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _articleSearchUrl;
        private readonly string _mostPopularUrl;

        public NytNewsAdapter(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["nytApiKey"] ?? throw new ArgumentNullException("nytApiKey not configured");
            _articleSearchUrl = configuration["articleSearchApi"] ?? throw new ArgumentNullException("articleSearchApi not configured");
            _mostPopularUrl = configuration["mostPopularUrl"] ?? throw new ArgumentNullException("mostPopularUrl not configured");
        }

        public async Task<IEnumerable<Article>> SearchArticlesAsync(string keyword, string? section = null, int page = 0)
        {
            var url = $"{_articleSearchUrl}?q={keyword}&page={page}&api-key={_apiKey}";

            if (!string.IsNullOrEmpty(section))
                url += $"&section_name={section}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            var docs = json.RootElement
                .GetProperty("response")
                .GetProperty("docs");

            return docs.EnumerateArray()
                .Select(doc => new Article
                {
                    NytId = doc.GetProperty("_id").GetString() ?? "",
                    Title = doc.GetProperty("headline").GetProperty("main").GetString() ?? "",
                    Abstract = doc.GetProperty("abstract").GetString() ?? "",
                    Section = doc.TryGetProperty("section_name", out var sectionProp) ? sectionProp.GetString() ?? "" : "",
                    Subsection = doc.TryGetProperty("subsection_name", out var subProp) ? subProp.GetString() ?? "" : "",
                    Author = doc.GetProperty("byline").TryGetProperty("original", out var byline) ? byline.GetString() ?? "" : "",
                    PublishedAt = DateTime.TryParse(doc.GetProperty("pub_date").GetString(), out var dt) ? dt : DateTime.UtcNow,
                    Url = doc.GetProperty("web_url").GetString() ?? "",
                    ThumbnailUrl = doc.TryGetProperty("multimedia", out var media) && media.GetArrayLength() > 0
                        ? $"https://static01.nyt.com/{media[0].GetProperty("url").GetString()}" : ""
                })
                .ToList();
        }

        public async Task<IEnumerable<Article>> GetMostPopularAsync(int period = 7)
        {
            var url = $"{_mostPopularUrl}/{period}.json?api-key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            var results = json.RootElement.GetProperty("results");
            var articles = new List<Article>();

            foreach (var result in results.EnumerateArray())
            {
                var nytId = result.TryGetProperty("uri", out var uriProp)
                    ? uriProp.GetString() ?? Guid.NewGuid().ToString()
                    : result.TryGetProperty("id", out var idProp)
                        ? idProp.GetRawText()
                        : Guid.NewGuid().ToString();

                var thumbnailUrl = string.Empty;
                if (result.TryGetProperty("media", out var media) && media.GetArrayLength() > 0)
                {
                    var mediaMeta = media[0].GetProperty("media-metadata");
                    if (mediaMeta.GetArrayLength() >= 3)
                    {
                        thumbnailUrl = mediaMeta[2].GetProperty("url").GetString() ?? string.Empty;
                    }
                    else if (mediaMeta.GetArrayLength() > 0)
                    {
                        thumbnailUrl = mediaMeta[0].GetProperty("url").GetString() ?? string.Empty;
                    }
                }

                var author = result.TryGetProperty("byline", out var bylineProp)
                    ? bylineProp.GetString() ?? string.Empty : string.Empty;

                var publishedDate = result.TryGetProperty("published_date", out var pubDateProp) &&
                                    DateTime.TryParse(pubDateProp.GetString(), out var dt) ? dt : DateTime.UtcNow;

                articles.Add(new Article
                {
                    NytId = nytId,
                    Title = result.TryGetProperty("title", out var titleProp) ? titleProp.GetString() ?? string.Empty : string.Empty,
                    Abstract = result.TryGetProperty("abstract", out var absProp) ? absProp.GetString() ?? string.Empty : string.Empty,
                    Section = result.TryGetProperty("section", out var sectionProp) ? sectionProp.GetString() ?? string.Empty : string.Empty,
                    Subsection = result.TryGetProperty("subsection", out var subProp) ? subProp.GetString() ?? string.Empty : string.Empty,
                    Author = author,
                    PublishedAt = publishedDate,
                    Url = result.TryGetProperty("url", out var urlProp) ? urlProp.GetString() ?? string.Empty : string.Empty,
                    ThumbnailUrl = thumbnailUrl
                });
            }

            return articles;
        }
    }
}
