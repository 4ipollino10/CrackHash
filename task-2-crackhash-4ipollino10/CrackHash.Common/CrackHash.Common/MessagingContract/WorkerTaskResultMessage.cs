namespace CrackHash.Common.MessagingContract;

public class WorkerTaskResultMessage
{
    public Guid RequestId { get; set; }
    
    public Guid TaskId { get; set; }

    public List<string>? Result { get; set; }

    public string? ErrorMessage { get; set; }
}