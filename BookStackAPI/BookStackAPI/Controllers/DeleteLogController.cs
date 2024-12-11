using Microsoft.AspNetCore.Mvc;
using BookStackAPI.Data;
using BookStackAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteLogController : ControllerBase
    {
        private readonly DataContext _context; 
        public DeleteLogController(DataContext context) 
        {
            _context = context; 
        }
        [HttpPost] 
        public async Task<IActionResult> PostDeleteLog([FromBody] Deleted_books deletionRecord) 
        {
            _context.Deleted_books.Add(deletionRecord); 
            await _context.SaveChangesAsync(); 
            return Ok(); }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deleted_books>>> GetDeletedBooks()
        {
            return await _context.Deleted_books.ToListAsync();
        }

        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<Deleted_books>>> SearchBooks(string query)
        {
            var deleted_books = await _context.Deleted_books.Where(b =>
            b.AuthorAndTitle.Contains(query) || b.User.Contains(query) || b.LossOrSold.Contains(query) || b.Notes.Contains(query)).ToListAsync();
            if (deleted_books == null || !deleted_books.Any())
            {
                return NotFound();
            }
            return Ok(deleted_books);
        }
    }
}
