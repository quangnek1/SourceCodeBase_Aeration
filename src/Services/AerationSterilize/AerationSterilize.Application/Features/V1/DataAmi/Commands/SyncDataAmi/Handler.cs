using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.Excel;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Helpers;
using Shared.DTOs.Excel;

namespace AerationSterilize.Application.Features.V1.DataAmi.Commands.SyncDataAmi;
internal class SyncDataAmiCommandHandler : ICommandHandler<SyncDataAmiCommand>
{
    private readonly IRepositoryBase<Domain.Entities.DataAmiQ411, int> _dataAmiRepository;
    private readonly IExcelReaderService _excelReaderService;

    public SyncDataAmiCommandHandler(IRepositoryBase<Domain.Entities.DataAmiQ411, int> dataAmiRepository,
        IExcelReaderService excelReaderService)
    {
        _dataAmiRepository = dataAmiRepository ?? throw new ArgumentNullException(nameof(dataAmiRepository));
        _excelReaderService = excelReaderService ?? throw new ArgumentNullException(nameof(excelReaderService));
    }

    public async Task<Result> Handle(SyncDataAmiCommand request, CancellationToken cancellationToken)
    {
        var data = await _excelReaderService
             .ReadWorkbookAsync(request.options.DataAmiQ411, 4, 5, cancellationToken);

        // Map DataAmiQ411
        var dataAmiEntities = data.Sheets.Where(sheet =>
                    sheet.Headers.ContainsKey("製品名") &&
                    sheet.Headers.ContainsKey("CHAMBER_A"))
                    .SelectMany(sheet =>
                        sheet.Rows.Where(x => x.Cells[1] != null).Select(row =>
                            MapToDataAmiQ411(row, sheet.Headers)))
                    .ToList();

        // key dùng để check tồn tại
        var drawingNumbers = dataAmiEntities
            .Select(x => x.DrawingNumber)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        // query 1 lần các bản ghi đã tồn tại trong db
        var existingKeys = await _dataAmiRepository
            .FindAll(x => drawingNumbers.Contains(x.DrawingNumber))
            .Select(x => new { x.DrawingNumber, x.CatalogCode, x.Destination })
            .ToListAsync(cancellationToken);

        var existingSet = existingKeys
            .Select(x => BuildKey(x.DrawingNumber, x.CatalogCode, x.Destination))
            .ToHashSet();

        // chỉ insert những bản ghi chưa tồn tại
        var insertList = dataAmiEntities
            .Where(x => existingSet.Add(BuildKey(x.DrawingNumber, x.CatalogCode, x.Destination)))
            .ToList();

        if (insertList.Count > 0)
        {
            _dataAmiRepository.AddList(insertList);
        }

        var result = new List<int>(2) { insertList.Count, dataAmiEntities.Count - insertList.Count };

        return Result.Success(new
        {
            Inserted = result[0],
            Skipped = result[1],
        });
    }

    private static string BuildKey(string? drawingNumber, string? catalogCode, string? destination)
    {
        return string.Join("|",
            drawingNumber?.Trim() ?? string.Empty,
            catalogCode?.Trim() ?? string.Empty,
            destination?.Trim() ?? string.Empty);
    }

    private Domain.Entities.DataAmiQ411 MapToDataAmiQ411(ExcelRowData row, Dictionary<string, int> headers)
    {
        return new Domain.Entities.DataAmiQ411
        {
            ProductName = ExcelHelper.GetString(row, headers, "製品名"),

            ProductInformation = ExcelHelper.GetString(row, headers, "Product Information"),

            DrawingNumber = ExcelHelper.GetString(row, headers, "図番"),

            CatalogCode = ExcelHelper.GetString(row, headers, "カタログコード"),

            Destination = ExcelHelper.GetString(row, headers, "仕向け先"),

            ChamberA = ExcelHelper.GetInt(row, headers, "CHAMBER_A"),

            ChamberB = ExcelHelper.GetInt(row, headers, "CHAMBER_B"),

            ChamberC = ExcelHelper.GetInt(row, headers, "CHAMBER_C"),

            ChamberD = ExcelHelper.GetInt(row, headers, "CHAMBER_D"),
        };
    }
}
