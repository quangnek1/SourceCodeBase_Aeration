using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Commands.UpdateAerationColumn;

public sealed class UpdateAerationColumnCommand : ICommand<AerationColumnDto>
{
    public int Id { get; private set; }
    public string ColumnName { get; set; }
    public bool? Status { get; set; }

    public void SetId(int id)
    {
        Id = id;
    }
}
