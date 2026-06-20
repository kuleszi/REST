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
                    ItemName = "iPhone 12",
                    ItemDescription = "Test auction dsecription",
                    Category = "Electronics/Phones",
                    StartPrice = 599.99m,
                    CurrentHighestBid = 599.99m,
                    StartedOn = DateTime.UtcNow,
                    EndedOn = DateTime.UtcNow.AddDays(7),
                    Status = AuctionStatus.Active,
                    OwnerId = user.Id
                };

                context.Auctions.Add(testAuction);
                await context.SaveChangesAsync();
            }
        }

    }
}

