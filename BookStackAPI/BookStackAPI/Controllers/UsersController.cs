using BookStackAPI.Data;
using BookStackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        
        [HttpGet("{username}")]
        public async Task<ActionResult<Users>> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<Users>>> SearchUsers(string query)
        {
            var users = await _context.Users.Where(b =>
            b.Username.Contains(query)).ToListAsync();
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpPost] 
        public async Task<ActionResult<Users>> CreateUser(Users user) 
        {
            _context.Users.Add(user); 
            await _context.SaveChangesAsync(); 
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user); 
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try 
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null) 
                {
                    return NotFound();
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent(); 
            }
            catch (Exception ex)
            { 
                return StatusCode(500, "Internal server error"); }
            }
    }
}

