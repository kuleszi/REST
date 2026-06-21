using L5_2025N_02.Model;
using L5_2025N_02.Database;
using Microsoft.EntityFrameworkCore;

namespace L5_2025N_02.Models;

public class SeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Users.AnyAsync())
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Testowy Admin",
                Email = "admin@test.pl",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Role = "USER"
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@test.pl");
        if (user is not null)
        {
            if (!await context.Auctions.AnyAsync())
            {

                var testAuction = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "iPhone 12",
                    Description = "Test auction dsecription",
                    Category = "Electronics/Phones",
                    StartPrice = 599.99m,
                    CurrentHighestBid = 599.99m,
                    StartedOn = DateTime.UtcNow,
                    EndedOn = DateTime.UtcNow.AddDays(7),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction2 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Samsung Galaxy S21",
                    Description = "Stan idealny, brak rys.",
                    Category = "Electronics/Phones",
                    StartPrice = 450.00m,
                    CurrentHighestBid = 450.00m,
                    StartedOn = DateTime.UtcNow,
                    EndedOn = DateTime.UtcNow.AddDays(5),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction3 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Vintage Leather Jacket",
                    Description = "Kurtka skórzana w stylu lat 90.",
                    Category = "Fashion/Clothing",
                    StartPrice = 120.00m,
                    CurrentHighestBid = 155.50m,
                    StartedOn = DateTime.UtcNow.AddDays(-1),
                    EndedOn = DateTime.UtcNow.AddDays(3),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction4 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Sony PlayStation 5",
                    Description = "Konsola z dwoma padami.",
                    Category = "Electronics/Gaming",
                    StartPrice = 300.00m,
                    CurrentHighestBid = 410.00m,
                    StartedOn = DateTime.UtcNow.AddDays(-2),
                    EndedOn = DateTime.UtcNow.AddDays(1),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction5 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Mountain Bike",
                    Description = "Rower górski, używany jeden sezon.",
                    Category = "Sports/Cycling",
                    StartPrice = 800.00m,
                    CurrentHighestBid = 800.00m,
                    StartedOn = DateTime.UtcNow,
                    EndedOn = DateTime.UtcNow.AddDays(10),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction6 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Coffee Maker",
                    Description = "Ekspres przelewowy, stan nowy.",
                    Category = "Home/Kitchen",
                    StartPrice = 50.00m,
                    CurrentHighestBid = 65.00m,
                    StartedOn = DateTime.UtcNow.AddDays(-3),
                    EndedOn = DateTime.UtcNow.AddDays(2),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction7 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Ergonomic Office Chair",
                    Description = "Wygodne krzesło biurowe z regulacją.",
                    Category = "Home/Furniture",
                    StartPrice = 200.00m,
                    CurrentHighestBid = 200.00m,
                    StartedOn = DateTime.UtcNow,
                    EndedOn = DateTime.UtcNow.AddDays(7),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction8 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Abstract Oil Painting",
                    Description = "Obraz olejny na płótnie, 50x70cm.",
                    Category = "Art/Decor",
                    StartPrice = 150.00m,
                    CurrentHighestBid = 190.00m,
                    StartedOn = DateTime.UtcNow.AddDays(-5),
                    EndedOn = DateTime.UtcNow.AddDays(0),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction9 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Mechanical Keyboard",
                    Description = "Klawiatura mechaniczna, podświetlenie RGB.",
                    Category = "Electronics/Accessories",
                    StartPrice = 80.00m,
                    CurrentHighestBid = 95.00m,
                    StartedOn = DateTime.UtcNow.AddDays(-1),
                    EndedOn = DateTime.UtcNow.AddDays(6),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                var testAuction10 = new Auction
                {
                    Id = Guid.NewGuid(),
                    Title = "Wireless Headphones",
                    Description = "Słuchawki bezprzewodowe z aktywną redukcją hałasu.",
                    Category = "Electronics/Audio",
                    StartPrice = 110.00m,
                    CurrentHighestBid = 110.00m,
                    StartedOn = DateTime.UtcNow,
                    EndedOn = DateTime.UtcNow.AddDays(4),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                context.Auctions.AddRange(testAuction, testAuction2, testAuction3, testAuction4, testAuction5, testAuction6, testAuction7, testAuction8, testAuction9);
                await context.SaveChangesAsync();
            }
        }

    }
}

