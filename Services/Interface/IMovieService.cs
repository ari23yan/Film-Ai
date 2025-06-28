using Film_Ai.Models.Entities;

namespace Film_Ai.Services.Interface
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task InsertManyAsync(IEnumerable<Movie> movies);
    }
}
