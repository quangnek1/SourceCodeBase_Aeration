using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Commands.CreateAerationColumn;

public sealed record CreateAerationColumnCommand(
    string ColumnName,
    bool? Status
) : ICommand<int>;
