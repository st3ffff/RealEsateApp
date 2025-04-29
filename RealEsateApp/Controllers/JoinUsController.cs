using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Controllers
{
    public class JoinUsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public JoinUsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadCV(IFormFile cvFile)
        {
            if (cvFile != null && cvFile.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email ?? "unknown@example.com";

                var safeEmail = email.Replace("@", "_at_").Replace(".", "_dot_");
                var fileName = $"{safeEmail}_{Path.GetFileName(cvFile.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cv", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(stream);
                }

                TempData["Success"] = "Успешно качихте вашето CV!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Моля, прикачете валиден файл.";
            return RedirectToAction(nameof(Index));
        }
    }
}
