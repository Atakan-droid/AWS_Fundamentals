using Customers.Api.Contracts.Messages;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DomainToMessageMapper
{
    public static CustomerMessages.CustomerCreated ToCustomerUpdatedMessage(this Customer customer)
    {
        return new CustomerMessages.CustomerCreated(Name: customer.FullName, Email: customer.Email);
    }
    
    public static CustomerMessages.CustomerUpdated ToCustomerCreatedMessage(this Customer customer)
    {
        return new CustomerMessages.CustomerUpdated(Name: customer.FullName, Email: customer.Email);
    }
    
    public static CustomerMessages.CustomerDeleted ToCustomerDeletedMessage(this Customer customer)
    {
        return new CustomerMessages.CustomerDeleted(Name: customer.FullName, Email: customer.Email);
    }
}