using System.Data.Common;
using L5_2025N_02.Database;
using L5_2025N_02.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using L5_2025N_02.Controllers.Dtos;
using L5_2025N_02.Model;
using L5_2025N_02.Exceptions;
using L5_2025N_02.Controllers.Dtos.Auctions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace L5_2025N_02.Tests;

public class UnitTestAuction
{
    //Test 1
    [Fact]
    public async Task CreateAsync_ShouldAddAuctionToDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "Test1_CreateAuctionDb").Options; 

        using var context = new AppDbContext(options); 

        var auctionService = new AuctionService(context);
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "SUPER_SECRET_KEY_THAT_HAS_AT_LEAST_32_CHARACTERS!"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"},
            {"Jwt:ExpiresMinutes", "60"}
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings!)
        .Build();
        var tokenService = new TokenService(config);
        var authService = new AuthService(context, tokenService);

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Test",
            Email = "john@test.pl",
            PasswordHash = "John123",
            Role = "USER"
        };

        context.Users.Add(existingUser);
        await context.SaveChangesAsync();

        var ownerExists = existingUser.Id;

         var request = new CreateAuctionRequest 

        { 
                    Title = "iPhone 12",
                    Description = "...",
                    Category = "Electronics/Phones",
                    // OwnerId = ownerExists,
                    // StartDateUtc = DateTime.UtcNow,
                    // EndDateUtc = DateTime.UtcNow.AddDays(7)
        }; 

        var result = await auctionService.CreateAsync(request, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("iPhone 12", result.Title);
    }

    //Test 2
    // [Fact]
    // public async Task CreateAsync_ShouldBadRequestException_WhenDatesProblem()
    // {
    //     var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "Test2_CreateAuctionWithWrongDatesDb").Options; 

    //     using var context = new AppDbContext(options); 

    //     var auctionService = new AuctionService(context);
    //     var inMemorySettings = new Dictionary<string, string> {
    //         {"Jwt:Key", "SUPER_SECRET_KEY_THAT_HAS_AT_LEAST_32_CHARACTERS!"},
    //         {"Jwt:Issuer", "TestIssuer"},
    //         {"Jwt:Audience", "TestAudience"},
    //         {"Jwt:ExpiresMinutes", "60"}
    //     };
    //     var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings!)
    //     .Build();
    //     var tokenService = new TokenService(config);
    //     var authService = new AuthService(context, tokenService);

    //     var existingUser = new User
    //     {
    //         Id = Guid.NewGuid(),
    //         Name = "John Test",
    //         Email = "john@test.pl",
    //         PasswordHash = "John123",
    //         Role = "USER"
    //     };

    //     context.Users.Add(existingUser);
    //     await context.SaveChangesAsync();

    //     var ownerExists = existingUser.Id;

    //      var request = new CreateAuctionRequest 

    //     { 
    //                 Title = "iPhone 12",
    //                 Description = "...",
    //                 Category = "Electronics/Phones",
    //                 // OwnerId = ownerExists,
    //                 // StartDateUtc = DateTime.UtcNow.AddDays(7),
    //                 // EndDateUtc = DateTime.UtcNow
    //     }; 

    //     var exception = await Assert.ThrowsAsync<BadRequestException>(() => auctionService.CreateAsync(request, CancellationToken.None));
    //     Assert.Equal("Data zakończenia musi być późniejsza niż data rozpoczęcia.", exception.Message);
    // }
}