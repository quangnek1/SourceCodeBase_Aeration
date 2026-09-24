using AerationSterilize.API.Abstractions;
using AerationSterilize.Application.Features.V1.AerationColumn.Commands.CreateAerationColumn;
using AerationSterilize.Application.Features.V1.AerationColumn.Commands.DeleteAerationColumn;
using AerationSterilize.Application.Features.V1.AerationColumn.Commands.UpdateAerationColumn;
using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumnById;
using AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumns;
using AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumnsIncludedData;
using Asp.Versioning;
using Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.API.Controllers.V1;

[ApiVersion(1)]
public class AerationColumnController : ApiController
{
    public AerationColumnController(ISender sender) : base(sender)
    {
    }

    [HttpGet(Name = "GetAerationColumns")]
    [ProducesResponseType(type: typeof(Result<PagedResult<AerationColumnDto>>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAerationColumns(
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var result = await Sender.Send(new GetAerationColumnsQuery(
            searchTerm,
            sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            pageIndex,
            pageSize));

        return Ok(result);
    }

    [HttpGet]
    [Route("GetAerationColumnsIncludedData")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAerationColumnsIncludedData(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAerationColumnsIncludedDataQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetAerationColumnById")]
    [ProducesResponseType(type: typeof(Result<AerationColumnDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAerationColumnById(int id)
    {
        var result = await Sender.Send(new GetAerationColumnByIdQuery(id));
        return Ok(result);
    }

    [HttpPost(Name = "CreateAerationColumn")]
    [ProducesResponseType(type: typeof(Result<int>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAerationColumn([FromBody] CreateAerationColumnCommand command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:int}", Name = "UpdateAerationColumn")]
    [ProducesResponseType(type: typeof(Result<AerationColumnDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAerationColumn(int id, [FromBody] UpdateAerationColumnCommand command)
    {
        command.SetId(id);
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:int}", Name = "DeleteAerationColumn")]
    [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAerationColumn(int id)
    {
        var result = await Sender.Send(new DeleteAerationColumnCommand(id));
        return Ok(result);
    }
}
