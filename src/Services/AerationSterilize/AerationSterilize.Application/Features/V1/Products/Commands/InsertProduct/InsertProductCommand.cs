using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Products.Commands.InsertProduct;
public sealed record InsertProductCommand(
    string Name,
    decimal Price,
    string Description) : ICommand<Guid>;

