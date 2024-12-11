using BookStackAPI.Data;
using BookStackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DataContext _context;
        public BooksController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpGet("search/{query}")] 
        public async Task<ActionResult<IEnumerable<Books>>> SearchBooks(string query) 
        {
            var books = await _context.Books.Where(b => 
            b.Author.Contains(query) || b.Title.Contains(query)).ToListAsync(); 
            if (books == null || !books.Any()) 
            {
                return NotFound(); 
            }
            return Ok(books); 
        }

        [HttpPost]
        public async Task<ActionResult<Books>> CreateBook(Books book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Books book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteBook(int id) 
        {
            var book = await _context.Books.FindAsync(id); 
            if (book == null) 
            {
                return NotFound(); 
            }
            _context.Books.Remove(book); 
            await _context.SaveChangesAsync(); 
            return NoContent(); }
    }
}