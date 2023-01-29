using Customers.Api.Contracts.Requests;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class ApiContractToDomainMapper
{
    public static Customer ToCustomer(this CustomerRequest request)
    {
        return new Customer
        {
            GitHubUsername = request.GitHubUsername,
            FullName = request.FullName,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            Id = Guid.NewGuid()
        };
    }

    public static Customer ToCustomer(this UpdateCustomerRequest request)
    {
        return new Customer
        {
            Id = request.Id,
            Email = request.Customer.Email,
            GitHubUsername = request.Customer.GitHubUsername,
            FullName = request.Customer.FullName,
            DateOfBirth = request.Customer.DateOfBirth
        };
    }
}
