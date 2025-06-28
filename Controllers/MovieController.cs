using Film_Ai.Models.Entities;
using Film_Ai.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Film_Ai.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public readonly IMovieService _movieService;
        private readonly ITMDbService _tmdb;
        public MovieController(IMovieService movieService, ITMDbService tmdb)
        {
            _movieService = movieService;
            _tmdb = tmdb;
        }

        [HttpGet]
        [Route("get-movies")]
        public async Task<ActionResult<List<Movie>>> GetList(string input)
        {
            var movies = await _tmdb.SearchMoviesAsync("war");
             await _movieService.InsertManyAsync(movies);
            return Ok();
        }
    }
}
