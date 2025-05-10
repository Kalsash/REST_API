using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API.Models;
using REST_API.Models.Contexts;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;
      
        public MoviesController(MoviesContext _db) {
            //.Database.EnsureCreated();
            // гарантируем, что база данных создана
            _context = _db;
            // загружаем данные из БД
           // _context.Movies.Load();
        }

        // GET: api/<MoviesController>
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
                return Ok(await _context.Movies.AsNoTracking().Take(1000).ToListAsync());
        }

        // GET api/<MoviesController>/id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }

        // POST api/<MoviesController>
        [Authorize(Roles = "admin")]    
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'MoviesContext.People'  is null.");
            }
            movie.MovieId = _context.Movies.ToListAsync().Result.MaxBy(m => m.MovieId).MovieId+1;
            _context.Movies.Add(movie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovieExists(movie.MovieId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok(CreatedAtAction("GetMovie", new { id = movie.MovieId }, movie));
        }

        // PUT api/<MoviesController>/5
        [Authorize(Roles = "admin")]
        //459494
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(long id, Movie movie)
        {
            movie.MovieId = id;
            _context.Entry(movie).State = EntityState.Modified;
            try {
                //_context.Movies.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!MovieExists(id))
                    return NotFound();
                else
                    throw;
            }
            
            return Ok(NoContent());
        }

        // DELETE api/<MoviesController>/id
        //[Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(long id)
        {
            // Явно включаем поддержку внешних ключей
            await _context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var movie = await _context.Movies.FindAsync(id);
                    if (movie == null)
                    {
                        return NotFound();
                    }

                    // Список всех таблиц, которые могут ссылаться на Movie
                    var relatedTables = new Dictionary<string, string>
            {
                {"movie_crew", "movie_id"},
                {"movie_cast", "movie_id"},
                {"movie_company", "movie_id"},
                {"movie_genres", "movie_id"},
                {"movie_keywords", "movie_id"},
                {"movie_languages", "movie_id"},
                {"production_country", "movie_id"}
            };

                    // Удаляем данные из всех связанных таблиц
                    foreach (var table in relatedTables)
                    {
                        await _context.Database.ExecuteSqlRawAsync(
                            $"DELETE FROM {table.Key} WHERE {table.Value} = {id}");
                    }

                    // Удаляем сам фильм
                    _context.Movies.Remove(movie);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error deleting movie: {ex.Message}");
                }
            }
        }

        private bool MovieExists(long id) =>
            _context.Movies.Any(e => e.MovieId == id);
    }
}
