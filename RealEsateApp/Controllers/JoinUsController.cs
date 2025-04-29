using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Controllers
{
    public class JoinUsController : Controller
    {
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cv", cvFile.FileName);
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
