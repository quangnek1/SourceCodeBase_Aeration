using AerationSterilize.API.Abstractions;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBox;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBoxingPosition;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanPosition;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanTag;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanWorkTable;
using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using Asp.Versioning;
using Contracts.Common.Messages;
using Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Helpers;

namespace AerationSterilize.API.Controllers.V1;

[ApiVersion(1)]
public class BoxingJobController : ApiController
{
    public BoxingJobController(ISender sender) : base(sender) { }

    //[HttpPost]
    //[Route("scan-workTable")]
    //[Authorize]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> ScanWorkTable(string tableCode, CancellationToken cancellationToken)
    //{
    //    ScanWorkTableCommand command = new ScanWorkTableCommand(tableCode);
    //    var result = await Sender.Send(command, cancellationToken);

    //    return Ok(result);
    //}

    //[HttpPost]
    //[Route("scan-position")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> ScanPosition(string positionCode, CancellationToken cancellationToken)
    //{
    //    var query = new ScanPositionCommand(positionCode);
    //    var result = await Sender.Send(query, cancellationToken);

    //    if (result.IsFailure)
    //    {
    //        return NotFound(result.Error);
    //    }

    //    return Ok(result);
    //}

    [HttpPost]
    [Route("scan-boxing-position")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ScanBoxingPosition(string code, CancellationToken cancellationToken)
    {
        var query = new ScanBoxingPositionCommand(code);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("scan-box")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ScanBox(string boxCode, CancellationToken cancellationToken)
    {
        var query = new ScanBoxCommand(boxCode);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("scan-tag")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ScanTag(string qrCode, CancellationToken cancellationToken)
    {
        // 1. Parse QR
        if (!ItemTagQrCode.TryParse(qrCode, out var tag))
        {
            return BadRequest(new Error("400", "Invalid QR Code."));
        }
        if (tag.Qty <= 10)
        {
            var result = await Sender.Send(new ScanTagCommand(qrCode), cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
        var boxResult = await Sender.Send(new ScanBoxCommand(qrCode), cancellationToken);

        if (boxResult.IsFailure)
        {
            return BadRequest(boxResult.Error);
        }

        return Ok(boxResult);
    }
}
