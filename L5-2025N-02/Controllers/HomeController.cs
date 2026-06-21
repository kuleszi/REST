using Microsoft.AspNetCore.Mvc;
using L5_2025N_02.Services;

namespace L5_2025N_02.Controllers;

public class HomeController(AuctionService auctionService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var auctions = await auctionService.GetAllAsync(null, null, null, false, 1, 10, ct);
        return View(auctions);
    }
}