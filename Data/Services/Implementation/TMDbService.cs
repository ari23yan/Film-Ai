using Film_Ai.Data.Services.Interface;
using Film_Ai.Models.Entities;
using Film_Ai.Tools;
using GoogleTranslateFreeApi;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Film_Ai.Data.Services.Implementation
{
    public class TMDbService : ITMDbService
    {
        private readonly HttpClient _httpClient;
        private readonly IMovieService _movieService;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        public TMDbService(HttpClient httpClient, IConfiguration configuration,
            IMovieService movieService)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDb:ApiKey"];
            _apiUrl = configuration["TMDb:ApiUrl"];
            _movieService = movieService;
        }

        public async Task<List<Movie>> GetMoviesAsync(string generId)
        {
            var url = $"{_apiUrl}{_apiKey}&with_genres=28";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var results = json["results"];
            var movies = new List<Movie>();
            foreach (var result in results!)
            {
                movies.Add(new Movie
                {
                    Poster = result["poster_path"]?.ToString() ?? "",
                    Title = result["title"]?.ToString() ?? "",
                    Description = result["overview"]?.ToString() ?? "",
                    Description_Fa = await Translator.Translate(result["overview"]?.ToString(), Language.English, Language.Persian) ?? "",
                    ReleaseYear = result["release_date"]?.ToString().Split('-')[0] ?? ""
                });
            }
            var moviesList = await _movieService.FilterNonExistingMoviesAsync(movies);
            return moviesList.ToList();
        }
    }
}
