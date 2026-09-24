using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AerationSterilize.Application.Features.V1.Batch.Events;
using AerationSterilize.Application.Features.V1.DataPlan.Events;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.PDF;
using Contracts.Services.QRCode;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.PDF;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.PrintBatchPdf;
internal class PrintBatchPdfCommandHandler : ICommandHandler<PrintBatchPdfCommand, PdfResultDto>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IPdfService _pdfService;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IMapper _mapper;
    private readonly IQrCodeServices _qrCodeServices;
    private readonly IPublisher _publisher;

    public PrintBatchPdfCommandHandler(IPdfService pdfService, ITemplateRenderer templateRenderer, IMapper mapper,
        IRepositoryBase<Domain.Entities.Batch, int> batchRepository, IQrCodeServices qrCodeServices, IPublisher publisher)
    {
        _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
        _templateRenderer = templateRenderer ?? throw new ArgumentNullException(nameof(templateRenderer));
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _qrCodeServices = qrCodeServices ?? throw new ArgumentNullException(nameof(qrCodeServices));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

    public async Task<Result<PdfResultDto>> Handle(PrintBatchPdfCommand request, CancellationToken cancellationToken)
    {
        var lamda = new Func<IQueryable<Domain.Entities.Batch>, IQueryable<Domain.Entities.Batch>>(q =>
        {
            return q.Include(x => x.BatchItems)
                    .Include(x => x.BatchInAerationPositions)
                    .ThenInclude(x => x.AerationPosition);
        });

        var batches = await _batchRepository
                .FindAll(x => request.BatchIds.Contains(x.Id), false, lamda)
                .ToListAsync(cancellationToken);

        var batchesDto = _mapper.Map<List<BatchDto>>(batches);

        var htmls = new List<string>();

        foreach (var item in batchesDto)
        {
            item.QrCodeSvg = await _qrCodeServices.GenerateQrCodeAsync(item.QRCode, cancellationToken);

            var html = await _templateRenderer.RenderAsync("F6112", item, "V8");
            htmls.Add(html);
        }
        var finalHtml = string.Join("", htmls);

        var pdfBytes = await _pdfService.GeneratePDFAsync(finalHtml, cancellationToken);

        var result = new PdfResultDto(
            pdfBytes,
            $"Batch_{DateTime.Now:yyyyMMddHHmmss}.pdf"
        );

        await _publisher.Publish(new UpdateCountPrintBatchWhenPrintPdfEvent(new Guid(), request.BatchIds), cancellationToken);

        return Result<PdfResultDto>.Success(result);
    }
}
