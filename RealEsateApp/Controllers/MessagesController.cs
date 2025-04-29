using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data;
using RealEstateApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealEstateApp.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly HouseRentingDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(HouseRentingDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 📥 Входящи съобщения
        public async Task<IActionResult> Inbox()
        {
            var userId = Guid.Parse(_userManager.GetUserId(User));
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return View(messages);
        }

        // 📤 Изпратени съобщения
        public async Task<IActionResult> Sent()
        {
            var userId = Guid.Parse(_userManager.GetUserId(User));
            var messages = await _context.Messages
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return View(messages);
        }

        // ✉️ Създаване на съобщение (форма)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUserId = Guid.Parse(_userManager.GetUserId(User));
            var allUsers = _context.Users
                .Where(u => u.Id != currentUserId)
                .ToList();

            var recipients = new List<SelectListItem>();

            foreach (var user in allUsers)
            {
                var isBroker = await _userManager.IsInRoleAsync(user, "Broker");

                // Ако си брокер, виждаш всички потребители
                // Ако не си брокер, виждаш само брокери
                if (User.IsInRole("Broker") || isBroker)
                {
                    recipients.Add(new SelectListItem
                    {
                        Value = user.Id.ToString(),
                        Text = user.Email
                    });
                }
            }

            ViewBag.Users = recipients;
            ViewBag.Placeholder = User.IsInRole("Broker")
                ? "-- Избери потребител --"
                : "-- Избери брокер --";

            return View();
        }



        // ✉️ Изпращане на съобщение
        [HttpPost]
        public async Task<IActionResult> Create(Message message)
        {
            message.SenderId = Guid.Parse(_userManager.GetUserId(User));
            message.SentAt = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
                var currentUserId = Guid.Parse(_userManager.GetUserId(User));
                var allUsers = _context.Users
                    .Where(u => u.Id != currentUserId)
                    .ToList();

                var brokers = new List<SelectListItem>();

                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, "Broker"))
                    {
                        brokers.Add(new SelectListItem
                        {
                            Value = user.Id.ToString(),
                            Text = user.Email
                        });
                    }
                }

                ViewBag.Users = brokers.AsEnumerable();

                return View(message);
            }

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Sent));
        }
    }
}
