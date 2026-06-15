using System.Data.Common;
using L5_2025N_02.Database;
using L5_2025N_02.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using L5_2025N_02.Controllers.Dtos;
using L5_2025N_02.Model;
using L5_2025N_02.Exceptions;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace L5_2025N_02.Tests;

public class UnitTestUser
{
    //Test 1
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "Test1_GetUserDb").Options; 

        using var context = new AppDbContext(options); 

        


        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "SUPER_SECRET_KEY_THAT_HAS_AT_LEAST_32_CHARACTERS!"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"},
            {"Jwt:ExpiresMinutes", "60"}
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings!)
        .Build();
        var tokenService = new TokenService(config);
        var userService = new UserService(context);

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

        var result = await userService.GetByIdAsync(existingUser.Id, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(existingUser.Email, result.Email);
    }
}

