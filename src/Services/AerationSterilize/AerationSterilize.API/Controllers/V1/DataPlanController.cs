using AerationSterilize.API.Abstractions;
using AerationSterilize.Application.Features.V1.DataAmi.Commands.SyncDataAmi;
using AerationSterilize.Application.Features.V1.DataAmi.Queries.GetDataAmi;
using AerationSterilize.Application.Features.V1.DataPlan.Commands.SyncData;
using AerationSterilize.Application.Features.V1.DataPlan.Queries.GetAllData;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Options;

namespace AerationSterilize.API.Controllers.V1;

[ApiVersion(1)]
public class DataPlanController : ApiController
{
    private readonly DataPlanOptions options;
    public DataPlanController(ISender sender, DataPlanOptions options) : base(sender)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    [HttpPost]
    [Route("sync-data-plan")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DataPlan(CancellationToken cancellationToken)
    {
        var command = new SyncDataCommand(options);
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Route("sync-data-ami")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DataAmi(CancellationToken cancellationToken)
    {
        var command = new SyncDataAmiCommand(options);
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("get-all-dataplan")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Gets([FromQuery] string? filter, CancellationToken cancellationToken)
    {
        var command = new GetAllDataQuery(filter);
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("get-all-ami")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDataAMIQ411([FromQuery] string? filter, CancellationToken cancellationToken)
    {
        var command = new GetDataAmiQuery(filter ?? string.Empty);
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

}
