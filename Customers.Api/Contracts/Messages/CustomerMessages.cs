namespace Customers.Api.Contracts.Messages;

public class CustomerMessages
{
    public record CustomerCreated(string Name, string Email);

    public record CustomerUpdated(string Name, string Email);

    public record CustomerDeleted(string Name, string Email);
    
}