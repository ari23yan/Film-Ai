using Film_Ai.Models.Entities;

namespace Film_Ai.Services.Interface
{
    public interface ITMDbService
    {
        Task<List<Movie>> SearchMoviesAsync(string query);
    }
}
