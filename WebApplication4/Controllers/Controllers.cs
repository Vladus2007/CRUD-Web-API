using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using WebApplication4.AppDbContext;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace WebApplication4.Controllers


{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class Controllers : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<Controllers> _logger;
        private readonly UserManager<UserModel> _userManager;
        public Controllers(ApplicationContext context,ILogger<Controllers> logger,UserManager<UserModel> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager; 
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        [HttpGet]
        public async Task<IActionResult> GetNotesAsync()
        {
            var userId = GetUserId();
           ListNotes listNotes = new ListNotes();
             listNotes.Notes = await _context.Notes.Where(u=>u.UserId.ToString()==userId).ToListAsync();
            return Ok(listNotes);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GiveNotesAsync([FromBody] NotesRequest NotesRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Note newNote = new Note();
            newNote.title = NotesRequest.title;
            newNote.description = NotesRequest.description;
            newNote.CreateAt = DateTime.Now;
            await _context.AddAsync(newNote);
           await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("ByTitle")]
        public async Task<IActionResult> GiveNotesAsyncBytitle([FromQuery] string title)
        {
            var userId=GetUserId();
            var notes = await _context.Notes
                .Where(u=>u.title==title).
                Where(u=>u.UserId.ToString()==userId).
                ToListAsync();
            if (notes == null) return NotFound($"Not Found with {title} title");
            return Ok(notes);
        }
        [HttpPut("Description")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDescription([FromBody] EditingDescriptionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var note = await _context.Notes
                    .FirstOrDefaultAsync(n => n.id == request.id && n.UserId.ToString() ==GetUserId());

                if (note == null)
                {
                    return NotFound($"Note with ID {request.id} not found");
                }

                note.description = request.description;
                note.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status409Conflict, "The record was modified by another user.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating note description");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating note");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating note description");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var note = await _context.Notes.FirstOrDefaultAsync(u => u.id == id && u.UserId.ToString() ==GetUserId());
                if (note == null) return NotFound();
                _context.Remove(note);
                await _context.SaveChangesAsync();

                return Ok();
            }
            
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error with remove or savechanges in database");
            }
            
        }
    }
}
