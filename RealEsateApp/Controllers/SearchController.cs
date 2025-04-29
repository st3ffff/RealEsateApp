using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services;

namespace RealEstateApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly HouseService houseService;

        public SearchController(HouseService houseService)
        {
            this.houseService = houseService;
        }

        public async Task<IActionResult> Index(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return View(new List<Models.House>());
            }

            var houses = await houseService.SearchAsync(keyword);
            ViewBag.Keyword = keyword;

            return View(houses);
        }
    }
}
