using L5_2025N_02.Model;

namespace L5_2025N_02.Controllers.Dtos.Auctions;

public class AuctionResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    // public decimal StartingPrice { get; set; }
    // public decimal CurrentHighestBid { get; set; }
    // public DateTime StartDateUtc { get; set; }
    // public DateTime EndDateUtc { get; set; }
    public AuctionStatus Status { get; set; }
    // public Guid OwnerId { get; set; }
}