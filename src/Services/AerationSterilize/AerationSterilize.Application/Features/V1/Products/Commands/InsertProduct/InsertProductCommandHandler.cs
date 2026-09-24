using Contracts.Common.Messages;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.Products.Commands.InsertProduct;
internal class InsertProductCommandHandler : ICommandHandler<InsertProductCommand, Guid>
{
    public Task<Result<Guid>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
