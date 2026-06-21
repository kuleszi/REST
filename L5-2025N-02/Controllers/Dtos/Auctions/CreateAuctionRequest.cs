using System.ComponentModel.DataAnnotations;

namespace L5_2025N_02.Controllers.Dtos.Auctions;

public class CreateAuctionRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = default!;

    [Required, MaxLength(5000)]
    public string Description { get; set; } = default!;

    [Required, MaxLength(100)]
    public string Category { get; set; } = default!;
}