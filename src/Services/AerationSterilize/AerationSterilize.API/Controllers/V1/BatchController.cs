using AerationSterilize.API.Abstractions;
using AerationSterilize.Application.Features.V1.Batch.Commands.AdjustAerationTime;
using AerationSterilize.Application.Features.V1.Batch.Commands.CreateF6112;
using AerationSterilize.Application.Features.V1.Batch.Commands.DeleteBatch;
using AerationSterilize.Application.Features.V1.Batch.Commands.InputAerationRoom;
using AerationSterilize.Application.Features.V1.Batch.Commands.OutputAerationRoom;
using AerationSterilize.Application.Features.V1.Batch.Commands.PrintBatchPdf;
using AerationSterilize.Application.Features.V1.Batch.Queries.GetAllBatchF6112;
using AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchF6112;
using AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchPrintPDF;
using AerationSterilize.Application.Features.V1.Batch.Queries.GetItemsProgress;
using Asp.Versioning;
using Contracts.Services.PDF;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AerationSterilize.API.Controllers.V1;

[ApiVersion(1)]
public class BatchController : ApiController
{
    private readonly IPdfService _pdfService;
    private readonly ITemplateRenderer _templateRenderer;

    public BatchController(ISender sender, IPdfService pdfService, ITemplateRenderer templateRenderer) : base(sender)
    {
        _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
        _templateRenderer = templateRenderer ?? throw new ArgumentNullException(nameof(templateRenderer));
    }

    [HttpGet]
    [Route("get-all-batch")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Batchs(string? searchTerm = null,
      string? sortColumn = null,
      string sortOrder = "Ascending",
      int pageIndex = 1,
      int pageSize = 10)
    {
        var result = await Sender.Send(new GetAllBatchF6112Query(searchTerm, sortColumn, sortOrder, pageIndex, pageSize));

        return Ok(result);
    }

    [HttpGet]
    [Route("{batchId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBatch(int batchId, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetBatchByIdQuery(batchId), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("GetBatchByQrcode")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBatchByQrcode(string qrcode, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetBatchByQrcodeQuery(qrcode), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("get-all-items")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BatchItems(string? searchTerm = null,
      string? sort = null,
      int pageIndex = 1,
      int pageSize = 10)
    {
        var result = await Sender.Send(new GetItemsProgressQuery(searchTerm, sort, pageIndex, pageSize));

        return Ok(result);
    }

    [HttpPost]
    [Route("create-f6112")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Batch(CreateF6112Command command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Route("input-aeration")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InputBatchAeration(InputAerationRoomCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Route("output-aeration")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> OutputBatchAeration(OutputAerationRoomCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Route("adjust-aeration-time")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdjustAerationTime(AdjustAerationTimeCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.Code == "404" ? NotFound(result.Error) : BadRequest(result.Error);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{batchId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBatch(int batchId, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new DeleteBatchCommand(batchId), cancellationToken);

        if (result.IsFailure)
            return result.Error.Code == "404" ? NotFound(result.Error) : BadRequest(result.Error);

        return Ok(result);
    }

    [HttpGet]
    [Route("get-batch-pdf")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBatchPdf(int id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetBatchByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        var batch = result.Value;

        var html = await _templateRenderer.RenderAsync("F6112", batch, "V8");
        var pdfBytes = await _pdfService.GeneratePDFAsync(html, cancellationToken);

        return File(pdfBytes, "application/pdf", $"Batch_{batch.BatchNo}.pdf");
    }

    [HttpGet]
    [Route("get-batch-print")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBatchPrint(string? searchTerm = null,
      string? sortColumn = null,
      string sortOrder = "Ascending",
      int pageIndex = 1,
      int pageSize = 10)
    {
        var query = new GetBatchPrintPDFQuery(searchTerm, sortColumn, sortOrder, pageIndex, pageSize);
        var result = await Sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("print-all-batch-pdf")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PrintAllBatchPdf(IReadOnlyList<int> ids, CancellationToken cancellationToken)
    {
        var command = new PrintBatchPdfCommand(ids);
        var result = await Sender.Send(command, cancellationToken);

        return File(result.Value.Content, "application/pdf", $"{result.Value.FileName}");
    }
}
