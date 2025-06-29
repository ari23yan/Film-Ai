using Film_Ai.Models.Entities;

namespace Film_Ai.Data.Services.Interface
{
    public interface ITMDbService
    {
        Task<List<Movie>> GetMoviesAsync(string query);
    }
}
