using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Journal.Models;

namespace Journal.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin-panel")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        [Route("add-user")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("add-user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountManager newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            var user = new IdentityUser { UserName = newUser.UserName };
            var password = newUser.Password;
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(newUser);
        }

        [Route("reset-password")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["UserName"] = user.UserName;
            var model = new AccountManager { Id = user.Id, UserName = user.UserName };
            return View(model);
        }

        [Route("reset-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(AccountManager account)
        {
            if (string.IsNullOrEmpty(account.Password))
            {
                ModelState.AddModelError("Password", "Das Passwort darf nicht leer sein.");
                return View(account);
            }

            var user = await _userManager.FindByIdAsync(account.Id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, account.Password);

            if (result.Succeeded)
            {
                TempData["Message"] = $"Das Passwort wurde erfolgreich geändert";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewData["UserName"] = user.UserName;
                return View(account);
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("delete-user")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new AccountManager { UserName = user.UserName, Id = user.Id };
            return View(model);
        }

        [Route("delete-user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    //TempData["Error"] = "Du darfst ein Admin-Konto nicht löschen!";
                    return RedirectToAction(nameof(Index));
                }
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}