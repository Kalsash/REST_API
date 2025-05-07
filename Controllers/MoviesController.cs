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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
                return Ok(await _context.Movies.AsNoTracking().ToListAsync());
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
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)   
                return NotFound();  
            //for
            var idMovieCrew = _context.MovieCrews.Where(x => x.MovieId == id);
            _context.MovieCrews.RemoveRange(idMovieCrew);
            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync(); 
            return Ok(movie);
        }

        private bool MovieExists(long id) =>
            _context.Movies.Any(e => e.MovieId == id);
    }
}
