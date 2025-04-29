using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Models;
using RealEstateApp.Services;
using System;

namespace RealEstateApp.Controllers
{
    public class HousesController : Controller
    {
        private readonly HouseService houseService;
        private readonly UserManager<ApplicationUser> userManager;

        public HousesController(HouseService houseService, UserManager<ApplicationUser> userManager)
        {
            this.houseService = houseService;
            this.userManager = userManager;
        }

        // Достъпно за всички
        public async Task<IActionResult> All(string? category, decimal? minPrice, decimal? maxPrice)
        {
            var houses = await houseService.GetAllAsync();

            if (!string.IsNullOrEmpty(category))
            {
                houses = houses
                    .Where(h => h.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (minPrice.HasValue)
            {
                houses = houses.Where(h => h.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                houses = houses.Where(h => h.Price <= maxPrice.Value).ToList();
            }

            ViewBag.SelectedCategory = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            ViewBag.AllCategories = houses
                .Select(h => h.Category.ToString())
                .Distinct()
                .ToList();

            return View(houses);
        }


        public async Task<IActionResult> ForRent(string? category, decimal? minPrice, decimal? maxPrice)
        {
            var houses = await houseService.GetHousesForRentAsync();

            if (!string.IsNullOrWhiteSpace(category))
            {
                houses = houses
                    .Where(h => h.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (minPrice.HasValue)
            {
                houses = houses.Where(h => h.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                houses = houses.Where(h => h.Price <= maxPrice.Value).ToList();
            }

            ViewBag.SelectedCategory = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.AllCategories = houses.Select(h => h.Category.ToString()).Distinct().ToList();

            return View(houses);
        }


        public async Task<IActionResult> ForSale(string? category, decimal? minPrice, decimal? maxPrice)
        {
            var houses = await houseService.GetHousesForSaleAsync();

            if (!string.IsNullOrWhiteSpace(category))
            {
                houses = houses
                    .Where(h => h.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (minPrice.HasValue)
            {
                houses = houses.Where(h => h.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                houses = houses.Where(h => h.Price <= maxPrice.Value).ToList();
            }

            ViewBag.SelectedCategory = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.AllCategories = houses.Select(h => h.Category.ToString()).Distinct().ToList();

            return View(houses);
        }

        [Authorize]
        public async Task<IActionResult> Mine()
        {
            var userId = Guid.Parse(userManager.GetUserId(User));
            var houses = await houseService.GetMineAsync(userId);
            return View(houses);
        }

        [Authorize]
        [Authorize(Roles = "Admin,Broker")]
        public IActionResult Add() => View();

        [HttpPost]
        [Authorize(Roles = "Admin,Broker")]
        public async Task<IActionResult> Add(House house)
        {
            if (!ModelState.IsValid)
            {
                return View(house);
            }

            house.AgentId = Guid.Parse(userManager.GetUserId(User));
            await houseService.AddAsync(house);

            TempData["Success"] = "Имотът беше добавен успешно!";
            return RedirectToAction(nameof(Mine));
        }



        [Authorize(Roles = "Admin,Broker")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var house = await houseService.GetByIdAsync(id);
            if (house == null)
            {
                return NotFound();
            }

            var userId = Guid.Parse(userManager.GetUserId(User));
            var isAdmin = User.IsInRole("Admin");

            if (house.AgentId != userId && !isAdmin)
            {
                return Unauthorized();
            }

            return View(house);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Broker")]
        public async Task<IActionResult> Edit(House house)
        {
            if (!ModelState.IsValid)
            {
                return View(house);
            }

            var userId = Guid.Parse(userManager.GetUserId(User));
            var isAdmin = User.IsInRole("Admin");

            var existingHouse = await houseService.GetByIdAsync(house.Id);
            if (existingHouse == null)
            {
                return NotFound();
            }

            if (existingHouse.AgentId != userId && !isAdmin)
            {
                return Unauthorized();
            }

            await houseService.UpdateAsync(house);
            TempData["Success"] = "Имотът беше успешно редактиран!";
            return RedirectToAction(nameof(Mine));
        }





        public async Task<IActionResult> Delete(Guid id)
        {
            await houseService.DeleteAsync(id);
            TempData["Success"] = "Имотът беше изтрит успешно!";
            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> DeleteMine(Guid id)
        {
            var userId = Guid.Parse(userManager.GetUserId(User));
            var house = await houseService.GetByIdAsync(id);

            if (house == null || house.AgentId != userId)
            {
                return Unauthorized();
            }

            await houseService.DeleteAsync(id);
            TempData["Success"] = "Имотът беше изтрит успешно!";
            return RedirectToAction(nameof(Mine));
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var house = await houseService.GetByIdAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            return View(house);
        }

    }
}
