using System.Text.Json;
using Core.Entities;
using Core.Ports.Services.NytNews;

namespace Application.Driven.Adapter
{
    public class NytNewsAdapter : INytNewsPort
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public NytNewsAdapter(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<IEnumerable<Article>> SearchArticlesAsync(string keyword, string? section = null, int page = 0)
        {
            var url = $"https://api.nytimes.com/svc/search/v2/articlesearch.json?q={keyword}&page={page}&api-key={_apiKey}";

            if (!string.IsNullOrEmpty(section))
                url += $"&section_name={section}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            var docs = json.RootElement
                .GetProperty("response")
                .GetProperty("docs");

            var articles = new List<Article>();

            foreach (var doc in docs.EnumerateArray())
            {
                articles.Add(new Article
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
                        ? $"https://static01.nyt.com/{media[0].GetProperty("url").GetString()}"
                        : ""
                });
            }

            return articles;
        }

        public async Task<IEnumerable<Article>> GetMostPopularAsync(int period = 7)
        {
            var url = $"https://api.nytimes.com/svc/mostpopular/v2/viewed/{period}.json?api-key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            var results = json.RootElement.GetProperty("results");
            var articles = new List<Article>();

            foreach (var result in results.EnumerateArray())
            {
                articles.Add(new Article
                {
                    NytId = result.GetProperty("id").GetRawText(), // pode ser int/string
                    Title = result.GetProperty("title").GetString() ?? "",
                    Abstract = result.GetProperty("abstract").GetString() ?? "",
                    Section = result.TryGetProperty("section", out var section) ? section.GetString() ?? "" : "",
                    Author = result.TryGetProperty("byline", out var byline) ? byline.GetString() ?? "" : "",
                    PublishedAt = DateTime.TryParse(result.GetProperty("published_date").GetString(), out var dt) ? dt : DateTime.UtcNow,
                    Url = result.GetProperty("url").GetString() ?? "",
                    ThumbnailUrl = result.TryGetProperty("media", out var media) && media.GetArrayLength() > 0
                        ? media[0].GetProperty("media-metadata")[0].GetProperty("url").GetString() ?? ""
                        : ""
                });
            }

            return articles;
        }
    }
}
