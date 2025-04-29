using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Models;
using RealEstateApp.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly HouseRentingDbContext dbContext;


        public AdminController(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    HouseRentingDbContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            var users = userManager.Users.ToList();

            var userRoles = new List<(ApplicationUser user, string role)>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add((user, roles.FirstOrDefault() ?? "Без роля"));
            }

            return View(userRoles);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles);
                await userManager.AddToRoleAsync(user, newRole);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Проверка дали потребителят има съобщения
            var hasMessages = dbContext.Messages.Any(m => m.SenderId == user.Id || m.ReceiverId == user.Id);

            if (hasMessages)
            {
                TempData["Error"] = "❌ Потребителят има свързани съобщения и не може да бъде изтрит.";
                return RedirectToAction(nameof(Index));
            }

            await userManager.DeleteAsync(user);
            TempData["Success"] = "✅ Потребителят беше успешно изтрит.";
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Messages()
        {
            var messages = dbContext.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderByDescending(m => m.SentAt)
                .ToList();

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var message = await dbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            dbContext.Messages.Remove(message);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Messages));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CVs()
        {
            var cvDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cv");
            var files = Directory.GetFiles(cvDirectory)
                                 .Select(f => Path.GetFileName(f))
                                 .ToList();

            return View(files);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCV(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cv", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                TempData["Success"] = "✅ Файлът беше успешно изтрит.";
            }
            else
            {
                TempData["Error"] = "❌ Файлът не беше намерен.";
            }

            return RedirectToAction("CVs");
        }


    }
}
