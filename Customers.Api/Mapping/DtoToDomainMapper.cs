using Customers.Api.Contracts.Data;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToDomain(this CustomerDto dto)
    {
        return new Customer
        {
            Id = dto.Id,
            GitHubUsername = dto.GitHubUsername,
            FullName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth
        };
    }
}
