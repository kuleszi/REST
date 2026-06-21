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

    public AuctionStatus Status { get; set; }
}