using AerationSterilize.API.Abstractions;
using AerationSterilize.Application.Features.V1.AerationPosition.Commands.CreateAerationPosition;
using AerationSterilize.Application.Features.V1.AerationPosition.Commands.DeleteAerationPosition;
using AerationSterilize.Application.Features.V1.AerationPosition.Commands.UpdateAerationPosition;
using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AerationSterilize.Application.Features.V1.AerationPosition.Queries.GetAerationPositionById;
using AerationSterilize.Application.Features.V1.AerationPosition.Queries.GetAerationPositions;
using Asp.Versioning;
using Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.API.Controllers.V1;

[ApiVersion(1)]
public class AerationPositionController : ApiController
{
    public AerationPositionController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetAerationPositions")]
    [ProducesResponseType(type: typeof(Result<PagedResult<AerationPositionDto>>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAerationPositions(
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var result = await Sender.Send(new GetAerationPositionsQuery(
            searchTerm,
            sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            pageIndex,
            pageSize));

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetAerationPositionById")]
    [ProducesResponseType(type: typeof(Result<AerationPositionDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAerationPositionById(int id)
    {
        var result = await Sender.Send(new GetAerationPositionByIdQuery(id));
        return Ok(result);
    }

    [HttpPost(Name = "CreateAerationPosition")]
    [ProducesResponseType(type: typeof(Result<int>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAerationPosition([FromBody] CreateAerationPositionCommand command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:int}", Name = "UpdateAerationPosition")]
    [ProducesResponseType(type: typeof(Result<AerationPositionDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAerationPosition(int id, [FromBody] UpdateAerationPositionCommand command)
    {
        command.SetId(id);
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:int}", Name = "DeleteAerationPosition")]
    [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAerationPosition(int id)
    {
        var result = await Sender.Send(new DeleteAerationPositionCommand(id));
        return Ok(result);
    }
}
