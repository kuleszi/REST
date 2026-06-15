using System.Data.Common;
using L5_2025N_02.Database;
using L5_2025N_02.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using L5_2025N_02.Controllers.Dtos;
using L5_2025N_02.Model;
using L5_2025N_02.Exceptions;


namespace L5_2025N_02.Tests;

public class UnitTestAuth
{
    //Test 1
    [Fact]
    public async Task RegisterAsync_ShouldAddUserToDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "Test1_RegisterUserDb").Options;

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
        var authService = new AuthService(context, tokenService);

        var request = new RegisterRequest
        {
            Name = "John Test",
            Email = "john@test.pl",
            Password = "John123"
        };

        var result = await authService.RegisterAsync(request);
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
    }

    //Test 2
    [Fact]
    public async Task RegisterAsync_ShouldBadRequestException_WhenEmailAlreadyExists()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "Test2_RegisterUserWithDuplicateEmailDb").Options;

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

        var request = new RegisterRequest
        {
            Name = "John NewTest",
            Email = "john@test.pl",
            Password = "NewJohn123"
        };

        var exception = await Assert.ThrowsAsync<BadRequestException>(() => authService.RegisterAsync(request));
        Assert.Equal("Email już istnieje!", exception.Message);
    }

    //Test 3
    [Fact]
    public async Task LoginAsync_ShouldReturnAuthResponse()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "Test3_LoginUserDb").Options;

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
        var authService = new AuthService(context, tokenService);

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Test",
            Email = "john@test.pl",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("John123")
        };

        context.Users.Add(existingUser);
        await context.SaveChangesAsync();

        var request = new LoginRequest
        {
            Email = existingUser.Email,
            Password = "John123"
        };
        
        var result = await authService.LoginAsync(request);
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
    }
}
