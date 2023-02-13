using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
{
    public required ILogger<CustomerDeletedHandler> _logger;
    private readonly GuidService _guidService;

    public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger, GuidService guidService)
    {
        _logger = logger;
        _guidService = guidService;
    }

    public Task<Unit> Handle(CustomerDeleted request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.FullName);
        _logger.LogInformation(_guidService.Id.ToString());
        return Unit.Task;
    }
}