using Xunit;
using RealEstateApp.Services;
using RealEstateApp.Models;
using RealEstateApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace RealEstateApp.Tests
{
    public class HouseServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddHouseToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<HouseRentingDbContext>()
                .UseInMemoryDatabase(databaseName: "AddHouseTestDb")
                .Options;

            var newHouse = new House
            {
                Title = "Нов имот",
                Description = "Описание за нов имот",
                Price = 123456,
                Category = PropertyCategory.Къща, // използвай твоя enum
                ImageUrl = "https://example.com/image.jpg",
                AgentId = Guid.NewGuid()
            };

            using (var context = new HouseRentingDbContext(options))
            {
                var service = new HouseService(context);

                // Act
                await service.AddAsync(newHouse);
            }

            // Assert
            using (var context = new HouseRentingDbContext(options))
            {
                var houseInDb = await context.Houses.FirstOrDefaultAsync(h => h.Title == "Нов имот");

                Assert.NotNull(houseInDb);
                Assert.Equal("Описание за нов имот", houseInDb.Description);
                Assert.Equal(123456, houseInDb.Price);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveHouseFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<HouseRentingDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteTestDb")
                .Options;

            var houseId = Guid.NewGuid();

            using (var context = new HouseRentingDbContext(options))
            {
                context.Houses.Add(new House
                {
                    Id = houseId,
                    Title = "Имот за триене",
                    AgentId = Guid.NewGuid(),
                    Description = "Описание на имота",
                    ImageUrl = "https://example.com/image.jpg",
                    Category = PropertyCategory.Къща,
                    Price = 120000
                });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new HouseRentingDbContext(options))
            {
                var service = new HouseService(context);
                await service.DeleteAsync(houseId);
            }

            // Assert
            using (var context = new HouseRentingDbContext(options))
            {
                var house = await context.Houses.FindAsync(houseId);
                Assert.Null(house); // имотът трябва да е изтрит
            }
        }


    }
}
