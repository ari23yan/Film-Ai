using Film_Ai.Models.Entities;
using Film_Ai.Services.Interface;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Film_Ai.Services.Implementation
{
    public class TMDbService : ITMDbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public TMDbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDb:ApiKey"];
        }

        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            var url = $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(query)}&api_key={_apiKey}&language=en-US";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var results = json["results"]?.Take(10);

            var movies = new List<Movie>();
            foreach (var result in results!)
            {
                movies.Add(new Movie
                {
                    Poster = result["poster_path"]?.ToString() ?? "",
                    Title = result["title"]?.ToString() ?? "",
                    Description = result["overview"]?.ToString() ?? "",
                    ReleaseYear = result["release_date"]?.ToString().Split('-')[0] ?? ""
            });
            }
            return movies;
        }
    }
}
