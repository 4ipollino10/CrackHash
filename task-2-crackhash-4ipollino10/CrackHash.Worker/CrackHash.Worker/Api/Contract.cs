namespace CrackHash.Worker.Api;

public class Contract
{
    public Guid RequestId { get; set; }
    
    public Guid TaskId { get; set; }
    
    public string Hash { get; set; } = null!;
    
    public int MaxLength { get; set; }
    
    public int Skip { get; set; }
}