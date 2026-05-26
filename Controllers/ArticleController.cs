using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Journal.Data;
using Journal.Models;
using Microsoft.AspNetCore.Authorization;

namespace Journal.Controllers
{
[AllowAnonymous]
[Route("articles")]
public class ArticleController : Controller
{
    private readonly JournalContext _context;

    public ArticleController(JournalContext context)
    {
       _context = context;
    }

    [Route("index")]
    public async Task<IActionResult> Index(int? pageNumber)
    {
    int pageSize = 12;
        var articles = _context.Note
            .Where(a => a.Type == NoteType.Article)
            .OrderByDescending(a => a.CreateDate);
        return View(await PaginatedList<Note>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize));
    }
    /*public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }*/
    [Route("name")]
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
                return Redirect("/fail");
            }

            return View(blogNote);
        }
}
}