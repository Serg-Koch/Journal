using System.Diagnostics;

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
public class HomeController : Controller
{
    private readonly JournalContext _context;

    public HomeController(JournalContext context)
    {
       _context = context;
    }
    //[Route("home")]
    public async Task<IActionResult> Index(int? pageNumber)
    {
        int pageSize = 12;
        var articles = _context.Note
            .Where(a => a.Type == NoteType.Blog)
            .OrderByDescending(a => a.CreateDate);
        return View(await PaginatedList<Note>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize));
    }
    [Route("about")]
    public async Task<IActionResult> About()
    {
        return View();
    }
    [Route("games/car-destroyer")]
     public async Task<IActionResult> CarDestroyer(bool game = false)
    {
        return View(game);
    }
    [Route("games/flying-cube")]
     public async Task<IActionResult> FlyingCube(bool game = false)
    {
        return View(game);
    }
    [Route("video")]
    public async Task<IActionResult> Video()
    {
        return View();
    }
    [Route("impressum")]
    public async Task<IActionResult> Impressum()
    {
        return View();
    }
    [Route("privacy")]
    public async Task<IActionResult> Privacy()
    {
        return View();
    }
    [Route("error")]
    public async Task<IActionResult> Error()
    {
        return View();
    }

    /*public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }*/

}
}