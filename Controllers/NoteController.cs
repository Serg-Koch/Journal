using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Journal.Data;
using Journal.Models;
using Microsoft.AspNetCore.Authorization;

namespace Journal.Controllers
{
    [Authorize]
    [Route("admin-panel")]
    public class BlogNoteController : Controller
    {
        private readonly JournalContext _context;

        public BlogNoteController(JournalContext context)
        {
            _context = context;
        }

        [Route("article")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Note.ToListAsync());
        }
        [Route("article/preview")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogNote = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogNote == null)
            {
                return NotFound();
            }

            return View(blogNote);
        }
        [Route("article/new")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Route("article/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Introduce,Content,IsPublished,CreateDate,Type,ThumbnailUrl")] Note blogNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogNote);
        }
        [Route("article/editing")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogNote = await _context.Note.FindAsync(id);
            if (blogNote == null)
            {
                return NotFound();
            }
            return View(blogNote);
        }
        [Route("article/editing")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Introduce,Content,IsPublished,Type,ThumbnailUrl")] Note blogNote)
        {
            var note = await _context.Note.FindAsync(id);
            if (id != blogNote.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    note.Title = blogNote.Title;
                    note.Introduce = blogNote.Introduce;
                    note.Content = blogNote.Content;
                    note.IsPublished = blogNote.IsPublished;
                    note.Type = blogNote.Type;
                    note.ThumbnailUrl = blogNote.ThumbnailUrl;
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogNoteExists(blogNote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogNote);
        }
        [Route("article/deleting")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogNote = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogNote == null)
            {
                return NotFound();
            }

            return View(blogNote);
        }
        [Route("article/deleting")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogNote = await _context.Note.FindAsync(id);
            if (blogNote != null)
            {
                _context.Note.Remove(blogNote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BlogNoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}
