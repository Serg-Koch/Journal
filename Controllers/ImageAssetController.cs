using System.Net;
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
using System.ComponentModel.Design;

namespace Journal.Controllers
{
    [Authorize]
    [Route("admin-panel/images")]
    public class ImageAssetController : Controller
    {
        private readonly JournalContext _context;
        private readonly IWebHostEnvironment _env;

        public ImageAssetController(JournalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Route("index")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 15;
            var images = _context.ImageAsset
                .OrderByDescending(image => image.UploadedAt);
            return View(await PaginatedList<ImageAsset>.CreateAsync(images.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [HttpGet]
        [Route("add-image")]
        [Authorize(Roles = "Admin")]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [Route("add-image")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(ImageAsset image, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Der File ist nicht gewählt.");
            }

            var asset = await ImageAsset.CreateFromFile(imageFile, _env.WebRootPath, image);

            if (asset == null)
            {
                return BadRequest("Dateiformat passt nicht.");
            }

            _context.ImageAsset.Add(asset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Route("editing")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var asset = await _context.ImageAsset.FindAsync(id);
            if (asset == null) return NotFound();

            return View(asset);
        }

        [Route("editing")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ImageAsset image)
        {
            if (id != image.Id) return NotFound();


            var assetInDb = await _context.ImageAsset.FindAsync(id);
            if (assetInDb == null) return NotFound();

            assetInDb.ImageName = image.ImageName;
            assetInDb.Type = image.Type;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assetInDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ImageAsset.Any(e => e.Id == image.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.ImageAsset
                .FirstOrDefaultAsync(pic => pic.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _context.ImageAsset.FindAsync(id);
            if (asset != null)
            {
                var filePath = Path.Combine(_env.WebRootPath, "uploads", asset.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.ImageAsset.Remove(asset);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
