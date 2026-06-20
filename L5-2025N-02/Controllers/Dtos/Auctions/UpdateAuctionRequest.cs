using System.ComponentModel.DataAnnotations;
using L5_2025N_02.Model;

namespace L5_2025N_02.Controllers.Dtos.Auctions;

public class UpdateAuctionRequest
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = default!;

    [Required, MaxLength(100)]
    public string Category { get; set; } = default!;

    // [Range(0.01, 999999999)]
    // public decimal StartingPrice { get; set; }

    // [Required]
    // public DateTime StartDateUtc { get; set; }

    // [Required]
    // public DateTime EndDateUtc { get; set; }

    public AuctionStatus Status { get; set; }
}