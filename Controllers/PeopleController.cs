using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PeopleController : ControllerBase
    {
        private readonly MoviesContext _context;

        public PeopleController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/People
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
          if (_context.People == null)
          {
              return NotFound();
          }
            return Ok(await _context.People.AsNoTracking().ToListAsync());
        }

        // GET: api/People/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
          if (_context.People == null)
          {
              return NotFound();
          }
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }
            _context.Entry(person).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                    return NotFound();
                else
                    throw;
            }
            return Ok(NoContent());
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
          if (_context.People == null)
          {
              return Problem("Entity set 'MoviesContext.People'  is null.");
          }
            _context.People.Add(person);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonExists(person.PersonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(CreatedAtAction("GetPerson", new { id = person.PersonId }, person));
        }

        // DELETE: api/People/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Удаляем MovieCasts через сырой SQL (без отслеживания)
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM movie_cast WHERE person_id = {0}", id);

                    // Удаляем MovieCrews
                    var movieCrews = await _context.MovieCrews
                        .Where(x => x.PersonId == id)
                        .ToListAsync();
                    _context.MovieCrews.RemoveRange(movieCrews);

                    // Удаляем саму персону
                    var person = await _context.People.FindAsync(id);
                    if (person == null) return NotFound();

                    _context.People.Remove(person);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        private bool PersonExists(long id)
        {
            return (_context.People?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
    }
}
