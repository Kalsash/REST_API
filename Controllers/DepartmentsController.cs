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
    public class DepartmentsController : ControllerBase
    {
        private readonly MoviesContext _context;

        public DepartmentsController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
          if (_context.Departments == null)
          {
              return NotFound();
          }
            return Ok(await _context.Departments.ToListAsync());
        }

        // GET: api/Departments/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(long id)
        {
          if (_context.Departments == null)
          {
              return NotFound();
          }
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(long id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
          if (_context.Departments == null)
          {
              return Problem("Entity set 'MoviesContext.Departments'  is null.");
          }
            _context.Departments.Add(department);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DepartmentExists(department.DepartmentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department));
        }

        // DELETE: api/Departments/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            if (_context.Departments == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            var idMovieCrew = _context.MovieCrews.Where(x => x.MovieId == id);
            _context.MovieCrews.RemoveRange(idMovieCrew);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Ok(NoContent());
        }

        private bool DepartmentExists(long id)
        {
            return (_context.Departments?.Any(e => e.DepartmentId == id)).GetValueOrDefault();
        }
    }
}
