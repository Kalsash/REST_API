using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API.Models;
using REST_API.Models.Contexts;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieCrewsController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MovieCrewsController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/MovieCrews
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieCrew>>> GetMovieCrews()
        {
          if (_context.MovieCrews == null)
          {
              return NotFound();
          }
            return Ok(await _context.MovieCrews.AsNoTracking().ToListAsync());
        }

        // GET: api/MovieCrews/5
        [Authorize]
        [HttpGet("{movies_id}/{person_id}/{departament_id}")]
        public async Task<ActionResult<MovieCrew>> GetMovieCrew(long? movies_id, long? person_id, long? dId)
        {
          if (_context.MovieCrews == null)
          {
              return NotFound();
          }

            var movieCrew = await _context.MovieCrews.
                FirstOrDefaultAsync(m => m.MovieId == movies_id && m.PersonId == person_id && m.DepartmentId == dId);

            if (movieCrew == null)
            {
                return NotFound();
            }

            return Ok(movieCrew);
        }

        // PUT: api/MovieCrews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles ="admin")]
        [HttpPut("{movies_id}/{person_id}/{departament_id}")]
        public async Task<IActionResult> PutMovieCrew(long? movies_id, long? person_id, long? departament_id, MovieCrew movieCrew)
        {

            if (movieCrew.MovieId != movies_id && movieCrew.PersonId != person_id && movieCrew.DepartmentId != departament_id)
            {
                return BadRequest();
            }

            _context.Entry(movieCrew).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieCrewExists(movies_id, person_id,departament_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(NoContent());
        }

        // POST: api/MovieCrews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<MovieCrew>> PostMovieCrew(MovieCrew movieCrew)
        {
          if (_context.MovieCrews == null)
          {
              return Problem("Entity set 'MoviesContext.MovieCrews'  is null.");
          }
          movieCrew.Movie = _context.Movies.Where(m => m.MovieId == movieCrew.MovieId).FirstOrDefault();
          movieCrew.Person = _context.People.Where(p => p.PersonId == movieCrew.PersonId).FirstOrDefault();
            movieCrew.Department = _context.Departments.Where(p => p.DepartmentId == movieCrew.DepartmentId).FirstOrDefault();
            _context.MovieCrews.Add(movieCrew);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovieCrewExists(movieCrew.MovieId, movieCrew.PersonId, movieCrew.DepartmentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(CreatedAtAction("GetMovieCrew", new { movieId = movieCrew.MovieId }, movieCrew));
        }

        // DELETE: api/MovieCrews/5
        [Authorize(Roles="admin")]
        [HttpDelete("{movies_id}/{person_id}/{departament_id}")]
        public async Task<IActionResult> DeleteMovieCrew(long? movies_id, long? person_id, long? departament_id)
        {
            if (_context.MovieCrews == null)
            {
                return NotFound();
            }
            var movieCrew = await _context.MovieCrews.
            FirstOrDefaultAsync(m => m.MovieId == movies_id && m.PersonId == person_id && m.DepartmentId == departament_id);
            if (movieCrew == null)
            {
                return NotFound();
            }

            _context.MovieCrews.Remove(movieCrew);
            await _context.Movies
                .Where(m => m.MovieId == movieCrew.MovieId)
                .ForEachAsync(m => _context.Movies.Remove(m));
               
            await _context.SaveChangesAsync();

            return Ok(NoContent());
        }

        private bool MovieCrewExists(long? mId, long? pId, long? dId)
        {
            return (_context.MovieCrews?.Any(m => m.MovieId == mId && m.PersonId == pId && m.DepartmentId == dId)).GetValueOrDefault();
        }
    }
}
