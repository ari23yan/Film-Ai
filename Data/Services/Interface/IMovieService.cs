using Film_Ai.Models.Entities;

namespace Film_Ai.Data.Services.Interface
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task InsertManyAsync(IEnumerable<Movie> movies);
        Task<IEnumerable<Movie>> FilterNonExistingMoviesAsync(IEnumerable<Movie> movies);
    }
}
