using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.Cache;
using Contracts.Services.Excel;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Helpers;
using Shared.DTOs.Excel;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.DataPlan.Commands.SyncData;
internal class SyncDataCommandHandler : ICommandHandler<SyncDataCommand>
{
    private readonly IRepositoryBase<Domain.Entities.DataPlan, int> _dataPlanRepository;
    private readonly IRepositoryBase<Domain.Entities.Setting, int> _settingRepository;
    private readonly IExcelReaderService _excelReaderService;
    private readonly ICacheService _cache;

    public SyncDataCommandHandler(IRepositoryBase<Domain.Entities.DataPlan, int> dataPlanRepository,
       IExcelReaderService excelReaderService, ICacheService cache, IRepositoryBase<Domain.Entities.Setting, int> settingRepository)
    {
        _dataPlanRepository = dataPlanRepository ?? throw new ArgumentNullException(nameof(dataPlanRepository));
        _excelReaderService = excelReaderService ?? throw new ArgumentNullException(nameof(excelReaderService));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
    }

    public async Task<Result> Handle(SyncDataCommand request, CancellationToken cancellationToken)
    {
        var setting = await _settingRepository.FindAll(null, tracking: false).FirstOrDefaultAsync(cancellationToken);
        var planCAGPath = setting?.PlanCAG ?? request.options.PlanCAG;
        var planPTCAPath = setting?.PlanPTCA ?? request.options.PlanPTCA;

        var task1 = _excelReaderService
           .ReadSheetAsync(planCAGPath, sheetIndex: 1, headerRow: 2, skipRow: 1, cancellationToken);

        var task2 = _excelReaderService
            .ReadSheetAsync(planPTCAPath, sheetIndex: 1, headerRow: 2, skipRow: 1, cancellationToken);

        var dataPlan = await Task.WhenAll(task1, task2);
        var dataCAG = await task1;
        var dataPTCA = await task2;

        var sheets = new[]
        {
            (Sheet: dataCAG, Type: "cag"),
            (Sheet: dataPTCA, Type: "ptca")
        };

        // Map DataPlan
        var dataPlanEntities = sheets
            .SelectMany(x =>
                x.Sheet.Rows.Select(row =>
                {
                    var entity = MapToDataPlan(row, x.Sheet.Headers);
                    entity.Type = x.Type;
                    entity.Status = DataStatus.Initial;
                    return entity;
                }))
            .Where(x => !string.IsNullOrWhiteSpace(x.InternalLot))
            .ToList();

        var duplicateLots = dataPlanEntities
            .GroupBy(x => x.InternalLot!.Trim())
            .Where(g => g.Count() > 1)
            .Select(g => new
            {
                InternalLot = g.Key,
                Count = g.Count()
            })
            .ToList();

        var duplicateLotKeys = duplicateLots.Select(x => x.InternalLot).ToHashSet();

        var validDataPlans = dataPlanEntities.Where(x => !duplicateLotKeys.Contains(x.InternalLot!.Trim())).ToList();

        CalculateMEChia(validDataPlans);

        // key dùng để check tồn tại
        var inernalLots = validDataPlans.Select(x => x.InternalLot!).Distinct().ToList();

        // query 1 lần
        var dbEntities = await _dataPlanRepository.FindAll(x => inernalLots.Contains(x.InternalLot!), tracking: true)
            .ToListAsync(cancellationToken);

        // convert dictionary
        var dbDict = dbEntities.ToDictionary(x => x.InternalLot!);

        var insertList = new List<Domain.Entities.DataPlan>();

        var updatedCount = 0;

        foreach (var excel in validDataPlans)
        {
            var key = excel.InternalLot!;

            // chưa có -> insert
            if (!dbDict.TryGetValue(key, out var dbEntity))
            {
                insertList.Add(excel);
                continue;
            }

            bool changed = false;

            // compare ME
            if (dbEntity.ME != excel.ME)
            {
                dbEntity.ME = excel.ME;
                changed = true;
            }

            // compare Ster
            if (dbEntity.Ster != excel.Ster)
            {
                dbEntity.Ster = excel.Ster;
                changed = true;
            }

            // compare InternalLot
            if (dbEntity.InternalLot != excel.InternalLot)
            {
                dbEntity.InternalLot = excel.InternalLot;
                changed = true;
            }

            if (changed)
            {
                updatedCount++;
            }
        }

        // insert mới
        if (insertList.Count > 0)
        {
            _dataPlanRepository.AddList(insertList);
        }


        return Result.Success(new
        {
            Inserted = insertList.Count,
            Updated = updatedCount,
            Skipped = dataPlanEntities.Count
                  - insertList.Count
                  - updatedCount,
            DuplicateLots = duplicateLots
        });
    }

    private Domain.Entities.DataPlan MapToDataPlan(ExcelRowData row, Dictionary<string, int> headers)
    {
        return new Domain.Entities.DataPlan
        {
            PONo = ExcelHelper.GetString(row, headers, "PO No."),
            TT = ExcelHelper.GetString(row, headers, "TT"),
            MaterialTypeC = ExcelHelper.GetString(row, headers, "Material No Type C"),
            ItemCodeTypeC = ExcelHelper.GetString(row, headers, "Item Code Type C"),
            PldOrd = ExcelHelper.GetString(row, headers, "PldOrd"),
            Material = ExcelHelper.GetString(row, headers, "Material No"),
            ItemCode = ExcelHelper.GetString(row, headers, "Item code"),
            ItemName = ExcelHelper.GetString(row, headers, "Item name"),
            Qty = ExcelHelper.GetInt(row, headers, "Q'ty"),
            CompleteDate = ExcelHelper.GetDate(row, headers, "Complete Date"),
            InternalLot = ExcelHelper.GetString(row, headers, "Internal Lot"),
            ExternalLot1 = ExcelHelper.GetString(row, headers, "External Lot Ster"),
            QtyInput = ExcelHelper.GetInt(row, headers, "Q'ty Input"),
            QATest = ExcelHelper.GetInt(row, headers, "QA test"),
            KeepAeration = ExcelHelper.GetInt(row, headers, "KEEP"),
            Destination = ExcelHelper.GetString(row, headers, "Destination"),
            Label = ExcelHelper.GetDate(row, headers, "External Lot Ster"),
            Bioburden = ExcelHelper.GetString(row, headers, "Label"),
            Seal = ExcelHelper.GetDate(row, headers, "Seal"),
            Ster = ExcelHelper.GetDate(row, headers, "Ster"),
            ME = ExcelHelper.GetString(row, headers, "MẺ"),
            Phase = ExcelHelper.GetString(row, headers, "Phase"),
            BOXING = ExcelHelper.GetString(row, headers, "BOXING"),
            TestEndotoxin = ExcelHelper.GetString(row, headers, "Test Endotoxin ( pcs)"),
            TestParticle = ExcelHelper.GetString(row, headers, "Test Particle ( pcs)"),
            LabelData = ExcelHelper.GetDouble(row, headers, "Label"),
            SealData = ExcelHelper.GetDouble(row, headers, "Seal"),
            BoxingData = ExcelHelper.GetDouble(row, headers, "Boxing"),
            ETD = ExcelHelper.GetDate(row, headers, "ETD"),
            Family = ExcelHelper.GetString(row, headers, "Family"),
            KeepStorage = ExcelHelper.GetInt(row, headers, "ngày lưu kho"),


        };
    }

    private void CalculateMEChia(List<Domain.Entities.DataPlan> dataPlans)
    {
        var meGroups = dataPlans
         .Where(x =>
             !string.IsNullOrWhiteSpace(x.ME) &&
             x.Ster.HasValue &&
             x.KeepStorage.HasValue)
         .GroupBy(x => new
         {
             Ster = x.Ster!.Value.Date,
             x.ME
         });

        foreach (var meGroup in meGroups)
        {
            // distinct KeepStorage
            var keepStorageMap = meGroup
                .Select(x => x.KeepStorage!.Value)
                .Distinct()
                .OrderBy(x => x)
                .Select((value, index) => new
                {
                    Value = value,
                    Index = index + 1
                })
                .ToDictionary(
                    x => x.Value,
                    x => x.Index);

            foreach (var item in meGroup)
            {
                var suffix = keepStorageMap[item.KeepStorage!.Value];

                item.MEChia = $"{item.ME}-{suffix}";
            }
        }
    }
}
