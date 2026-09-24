using Contracts.Common.Messages;
using Shared.DTOs.PDF;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.PrintBatchPdf;
public sealed record PrintBatchPdfCommand(IReadOnlyList<int> BatchIds) : ICommand<PdfResultDto>;

