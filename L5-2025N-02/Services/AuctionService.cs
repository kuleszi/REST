using L5_2025N_02.Controllers.Dtos.Auctions;
using L5_2025N_02.Database;
using L5_2025N_02.Model;
using Microsoft.EntityFrameworkCore;

namespace L5_2025N_02.Services;

public class AuctionService(AppDbContext context)
{
    public async Task<AuctionResponse> CreateAsync(CreateAuctionRequest request, CancellationToken ct)
    {
        var auction = new Auction
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Status = AuctionStatus.Active
        };

        context.Auctions.Add(auction);
        await context.SaveChangesAsync(ct);

        return MapToResponse(auction);
    }

    public async Task<AuctionResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var auction = await context.Auctions.FirstOrDefaultAsync(x => x.Id == id, ct);
        return auction is null ? null : MapToResponse(auction);
    }

    public async Task<IReadOnlyList<AuctionResponse>> GetAllAsync(
        string? category, 
        AuctionStatus? status, 
        string? sortBy, 
        bool isDescending, 
        int pageNumber, 
        int pageSize, 
        CancellationToken ct)
    {
        var query = context.Auctions.AsQueryable();

        
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(x => x.Category == category);

        if (status.HasValue)
            query = query.Where(x => x.Status == status);

       
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "title" => isDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                "id" => isDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                _ => query.OrderBy(x => x.Id) 
            };
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AuctionResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Category = x.Category,
                Status = x.Status
            })
            .ToListAsync(ct);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateAuctionRequest request, CancellationToken ct)
    {
        var auction = await context.Auctions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (auction is null) return false;

        auction.Title = request.Title;
        auction.Description = request.Description;

        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var auction = await context.Auctions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (auction is null) return false;

        context.Auctions.Remove(auction);
        await context.SaveChangesAsync(ct);
        return true;
    }

    private static AuctionResponse MapToResponse(Auction auction) => new()
    {
        Id = auction.Id,
        Title = auction.Title,
        Description = auction.Description,
        Category = auction.Category,
        Status = auction.Status
    };
}