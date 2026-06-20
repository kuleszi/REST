namespace L5_2025N_02.Model;

public class Auction
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    
    public decimal StartPrice { get; set; }
    public decimal CurrentHighestBid { get; set; }
    
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    
    public AuctionStatus Status { get; set; }
    
    public Guid OwnerId { get; set; } = default!;
    public User  Owner { get; set; } = default!;

    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
}


public enum AuctionStatus
{
    Draft = 0,
    Active = 1,
    Finished = 2,
    Canceled = 3
}