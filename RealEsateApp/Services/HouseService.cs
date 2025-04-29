using RealEstateApp.Data;
using RealEstateApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RealEstateApp.Services
{
    public class HouseService
    {
        private readonly HouseRentingDbContext context;

        public HouseService(HouseRentingDbContext context)
        {
            this.context = context;
        }

        public async Task<List<House>> GetAllAsync()
        {
            return await context.Houses
                .Include(h => h.Agent) // 👈 зареждаме брокера
                .ToListAsync();
        }

        public async Task<List<House>> GetMineAsync(Guid agentId)
        {
            return await context.Houses
                .Where(h => h.AgentId == agentId)
                .Include(h => h.Agent) // 👈 зареждаме брокера
                .ToListAsync();
        }

        public async Task<List<House>> GetHousesForRentAsync()
        {
            return await context.Houses
                .Where(h => h.PropertyType == PropertyType.ForRent)
                .Include(h => h.Agent) // 👈 зареждаме брокера
                .ToListAsync();
        }

        public async Task<List<House>> GetHousesForSaleAsync()
        {
            return await context.Houses
                .Where(h => h.PropertyType == PropertyType.ForSale)
                .Include(h => h.Agent) // 👈 зареждаме брокера
                .ToListAsync();
        }

        public async Task<House> GetByIdAsync(Guid id)
        {
            return await context.Houses
                .Include(h => h.Agent) // 👈 зареждаме брокера
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddAsync(House house)
        {
            await context.Houses.AddAsync(house);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(House house)
        {
            var existing = await context.Houses.FindAsync(house.Id);

            if (existing == null)
            {
                return;
            }

            existing.Title = house.Title;
            existing.Description = house.Description;
            existing.Price = house.Price;
            existing.ImageUrl = house.ImageUrl;
            existing.PropertyType = house.PropertyType;
            existing.Category = house.Category;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var house = await context.Houses.FindAsync(id);
            if (house != null)
            {
                context.Houses.Remove(house);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<House>> SearchAsync(string keyword)
        {
            return await context.Houses
                .Where(h => h.Title.Contains(keyword) || h.Description.Contains(keyword))
                .Include(h => h.Agent) // 👈 зареждаме брокера
                .ToListAsync();
        }
    }
}
