using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Commands.UpdateAerationPosition;

public sealed class UpdateAerationPositionCommand : ICommand<AerationPositionDto>
{
    public int Id { get; private set; }
    public string PositionCode { get; set; }
    public string? Image { get; set; }
    public int? Status { get; set; }
    public int AerationColumnId { get; set; }

    public void SetId(int id)
    {
        Id = id;
    }
}
