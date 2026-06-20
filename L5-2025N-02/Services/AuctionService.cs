using L5_2025N_02.Controllers.Dtos.Auctions;
using L5_2025N_02.Database;
using L5_2025N_02.Exceptions;
using L5_2025N_02.Model;
using Microsoft.EntityFrameworkCore;

namespace L5_2025N_02.Services;

public class AuctionService(AppDbContext db)
{
    public async Task<AuctionResponse> CreateAsync(CreateAuctionRequest request, CancellationToken ct)
    {
        var ownerExists = await db.Users.AnyAsync(x => x.Id == request.OwnerId, ct);
        if (!ownerExists)
            throw new BadRequestException("Właściciel aukcji nie istnieje.");

        if (request.EndDateUtc <= request.StartDateUtc)
            throw new BadRequestException("Data zakończenia musi być późniejsza niż data rozpoczęcia.");

        var auction = new Auction
        {
            Id = Guid.NewGuid(),
            ItemName = request.ItemName,
            ItemDescription = request.Description,
            Category = request.Category,
            StartPrice = request.StartingPrice,
            CurrentHighestBid = request.StartingPrice,
            StartedOn = request.StartDateUtc,
            EndedOn = request.EndDateUtc,
            OwnerId = request.OwnerId,
            Status = AuctionStatus.Active
        };

        db.Auctions.Add(auction);
        await db.SaveChangesAsync(ct);

        return Map(auction);
    }

    public async Task<AuctionResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var auction = await db.Auctions.FirstOrDefaultAsync(x => x.Id == id, ct);
        return auction is null ? null : Map(auction);
    }

    public async Task<IReadOnlyList<AuctionResponse>> GetAllAsync(string? category, AuctionStatus? status, CancellationToken ct)
    {
        var query = db.Auctions.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(x => x.Category == category);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        return await query
            .OrderByDescending(x => x.StartedOn)
            .Select(x => new AuctionResponse
            {
                Id = x.Id,
                ItemName = x.ItemName,
                Description = x.ItemDescription,
                Category = x.Category,
                StartingPrice = x.StartPrice,
                CurrentHighestBid = x.CurrentHighestBid,
                StartDateUtc = x.StartedOn,
                EndDateUtc = x.EndedOn,
                Status = x.Status,
                OwnerId = x.OwnerId
            })
            .ToListAsync(ct);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateAuctionRequest request, CancellationToken ct)
    {
        var auction = await db.Auctions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (auction is null)
            return false;

        if (request.EndDateUtc <= request.StartDateUtc)
            throw new BadRequestException("Data zakończenia musi być późniejsza niż data rozpoczęcia.");

        if (request.StartingPrice > auction.CurrentHighestBid && auction.Bids.Any())
            throw new BadRequestException("Nie można ustawić ceny wywoławczej powyżej aktualnej najwyższej oferty.");

        auction.ItemName = request.ItemName;
        auction.ItemDescription = request.Description;
        auction.Category = request.Category;
        auction.StartPrice = request.StartingPrice;
        auction.StartedOn = request.StartDateUtc;
        auction.EndedOn = request.EndDateUtc;
        auction.Status = request.Status;

        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var auction = await db.Auctions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (auction is null)
            return false;

        db.Auctions.Remove(auction);
        await db.SaveChangesAsync(ct);
        return true;
    }

    private static AuctionResponse Map(Auction auction) => new()
    {
        Id = auction.Id,
        ItemName = auction.ItemName,
        Description = auction.ItemDescription,
        Category = auction.Category,
        StartingPrice = auction.StartPrice,
        CurrentHighestBid = auction.CurrentHighestBid,
        StartDateUtc = auction.StartedOn,
        EndDateUtc = auction.EndedOn,
        Status = auction.Status,
        OwnerId = auction.OwnerId
    };
}