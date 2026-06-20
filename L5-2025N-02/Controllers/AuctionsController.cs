using L5_2025N_02.Controllers.Dtos.Auctions;
using L5_2025N_02.Controllers.Dtos.Bids;
using L5_2025N_02.Model;
using L5_2025N_02.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L5_2025N_02.Controllers;

[Authorize]
[ApiController]
[Route("auctions")]
public class AuctionsController(AuctionService auctionService, BidService bidService, ILogger<AuctionsController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AuctionResponse>> Create(
        [FromBody] CreateAuctionRequest request,
        CancellationToken ct)
    {
        logger.LogInformation("Creating a new auction");
        var result = await auctionService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuctionResponse>> GetById(Guid id, CancellationToken ct)
    {
        logger.LogInformation("Getting auction by id");
        var result = await auctionService.GetByIdAsync(id, ct);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuctionResponse>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] AuctionStatus? status,
        [FromQuery] string? sortBy,           // Nowość: pole do sortowania
        [FromQuery] bool isDescending = false, // Nowość: kierunek sortowania
        [FromQuery] int pageNumber = 1,        // Paginacja
        [FromQuery] int pageSize = 10,         // Paginacja
        CancellationToken ct = default)
    {
        logger.LogInformation("Getting all auctions with pagination and sorting");

        
        var result = await auctionService.GetAllAsync(category, status, sortBy, isDescending, pageNumber, pageSize, ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(
        Guid id,
        [FromBody] UpdateAuctionRequest request,
        CancellationToken ct)
    {
        logger.LogInformation("Updating a auction");
        var updated = await auctionService.UpdateAsync(id, request, ct);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
    {
        logger.LogInformation("Deleting auction by id");
        var deleted = await auctionService.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id:guid}/bids")]
    public async Task<ActionResult<BidResponse>> PlaceBid(
        Guid id,
        [FromBody] CreateBidRequest request,
        CancellationToken ct)
    {
        logger.LogInformation("Placing a new bid");
        var result = await bidService.PlaceBidAsync(id, request, ct);
        return Ok(result);
    }
}