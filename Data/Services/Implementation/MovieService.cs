using Film_Ai.Data.DbContext;
using Film_Ai.Data.Services.Interface;
using Film_Ai.Models.Entities;
using MongoDB.Driver;

namespace Film_Ai.Data.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly MongoDbContext _DbContext;

        public MovieService(MongoDbContext context)
        {
            _DbContext = context;
        }

        public async Task<IEnumerable<Movie>> FilterNonExistingMoviesAsync(IEnumerable<Movie> movies)
        {
            var filters = movies
                .Select(m => Builders<Movie>.Filter.And(
                    Builders<Movie>.Filter.Eq(x => x.Title, m.Title),
                    Builders<Movie>.Filter.Eq(x => x.ReleaseYear, m.ReleaseYear)))
                .ToList();

            var combinedFilter = Builders<Movie>.Filter.Or(filters);

            var existingMovies = await _DbContext.Movie
                .Find(combinedFilter)
                .Project(m => new { m.Title, m.ReleaseYear })
                .ToListAsync();

            return movies.Where(m => !existingMovies
                .Any(em => em.Title == m.Title && em.ReleaseYear == m.ReleaseYear));
        }



        public async Task<IEnumerable<Movie>> GetAllAsync() =>
           await _DbContext.Movie.Find(_ => true).ToListAsync();

        public async Task InsertManyAsync(IEnumerable<Movie> movies) =>
           await _DbContext.Movie.InsertManyAsync(movies);


    }
}
