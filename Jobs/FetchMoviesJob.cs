using Film_Ai.Data.Services.Interface;
using Film_Ai.Models.Entities;
using Quartz;

namespace Film_Ai.Jobs
{
    public class FetchMoviesJob: IJob
    {

        private readonly ITMDbService _tmdb;
        private readonly IMovieService _movieService;

        public FetchMoviesJob(ITMDbService tmdb, IMovieService movieService)
        {
            _tmdb = tmdb;
            _movieService = movieService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var keywords = new[] { "war", "drama", "romance", "sad" };

            var allMovies = new List<Movie>();

            foreach (var keyword in keywords)
            {
                var movies = await _tmdb.GetMoviesAsync(keyword);
                allMovies.AddRange(movies);
            }
            await _movieService.InsertManyAsync(allMovies);
        }
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
