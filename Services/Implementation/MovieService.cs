using Film_Ai.Data.DbContext;
using Film_Ai.Models.Entities;
using Film_Ai.Services.Interface;
using MongoDB.Driver;

namespace Film_Ai.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly MongoDbContext _DbContext;

        public MovieService(MongoDbContext context)
        {
            _DbContext = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync() =>
             _DbContext.Movie.Find(_ => true).ToEnumerable();
        public async Task InsertManyAsync(IEnumerable<Movie> movies)
        {
            try
            {
                await _DbContext.Movie.InsertManyAsync(movies);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
