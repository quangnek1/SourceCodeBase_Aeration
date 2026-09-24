using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using Contracts.Common.Messages;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchPrintPDF;
public sealed record GetBatchPrintPDFQuery(string? searchTerm,
    string? sortColumn,
    SortOrder? SortOrder,
    int PageIndex, int PageSize) : IQuery<PagedResult<BatchPrintPdfDto>>;
