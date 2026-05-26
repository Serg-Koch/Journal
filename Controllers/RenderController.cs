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
[Route("renders")]
public class RenderController : Controller
{
    private readonly JournalContext _context;

    public RenderController(JournalContext context)
    {
       _context = context;
    }

    //[Route("index")]
    public async Task<IActionResult> Index(int? pageNumber)
    {
        int pageSize = 9;
        var articles = _context.ImageAsset
            .Where(a => a.Type == ImageType.Render)
            .OrderByDescending(a => a.UploadedAt);
        return View(await PaginatedList<ImageAsset>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize));
    }
}
}