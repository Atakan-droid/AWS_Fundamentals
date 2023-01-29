namespace SQSPublisher;

public class CustomerCreated
{
    public required Guid CustomerId { get; set; }
    
    public required string Name { get; set; }
    
    public required string Email { get; set; }
    
    
    public required string GituhUserName { get; set; }
    
    public required DateTime DateOfBirth { get; set; }
}