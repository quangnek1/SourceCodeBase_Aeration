using AerationSterilize.Application.Features.V1.DataPlan.Events;
using AerationSterilize.Domain.Entities;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.CreateF6112;
internal class CreateF6112CommandHandler : ICommandHandler<CreateF6112Command>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepositoryBase;
    private readonly IRepositoryBase<Domain.Entities.DataPlan, int> _dataPlanRepository;
    private readonly IRepositoryBase<DataAmiQ411, int> _amiRepository;
    private readonly IPublisher _publisher;

    public CreateF6112CommandHandler(
        IRepositoryBase<Domain.Entities.Batch, int> batchRepositoryBase,
        IRepositoryBase<Domain.Entities.DataPlan, int> dataPlanRepository,
        IPublisher publisher,
        IRepositoryBase<DataAmiQ411, int> amiRepository)
    {
        _batchRepositoryBase = batchRepositoryBase ?? throw new ArgumentNullException(nameof(batchRepositoryBase));
        _dataPlanRepository = dataPlanRepository ?? throw new ArgumentNullException(nameof(dataPlanRepository));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _amiRepository = amiRepository ?? throw new ArgumentNullException(nameof(amiRepository));
    }

    public async Task<Result> Handle(CreateF6112Command request, CancellationToken cancellationToken)
    {
        // Check Batch đã tồn tại chưa.
        var batchExists = await _batchRepositoryBase
              .FindSingleAsync(x => x.SterilizeDate == request.SterilizeDate && x.BatchNo == request.batchNo, cancellationToken);

        if (batchExists is not null)
            return Result.Failure(new Error("500", "Mẻ đã tồn tại."));

        var dataPlans = await _dataPlanRepository
               .FindAll(x => x.Ster == request.SterilizeDate && x.MEChia == request.batchNo, tracking: true)
               .ToListAsync(cancellationToken);

        if (dataPlans.Count() == 0)
            return Result.Failure(new Error("500", "Không có dữ liệu để tạo mẻ."));

        var drawingNumbers = dataPlans.Select(dp => GetDrawingNumber(dp.ItemCode))
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();

        var catalogCodes = dataPlans
            .Select(dp => dp.ItemName)
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();

        var amiList = await _amiRepository
            .FindAll(x => drawingNumbers.Contains(x.DrawingNumber)
                       && catalogCodes.Contains(x.CatalogCode))
            .ToListAsync(cancellationToken);

        var amiLookup = amiList
            .GroupBy(x => (x.DrawingNumber, x.CatalogCode))
            .ToDictionary(g => g.Key, g => g.First());

        var keepDay = dataPlans.FirstOrDefault().KeepStorage ?? 0;
        var dataStatus = DataStatus.Created;

        var batchItems = dataPlans.Select(dp =>
        {
            var key = (GetDrawingNumber(dp.ItemCode), dp.ItemName);
            amiLookup.TryGetValue(key, out var ami);   // gọi 1 lần, dùng 4 chỗ

            return new BatchItem
            {
                DataPlanId = dp.Id,
                NumberRank = dp.TT,
                Material = dp.Material,
                DrawingListNo = dp.DrawingListNo,
                ItemName = dp.ItemName,
                Destination = dp.Destination,
                InternalLot = dp.InternalLot,
                ExternalLot1 = dp.ExternalLot1,
                QtyInput = dp.QtyInput ?? 0,
                KeepSample = dp.KeepAeration.ToString(),
                Family = dp.Family ?? string.Empty,
                DeliveryDatePlan = dp.ETD,

                A = ami?.ChamberA.ToString(),
                B = ami?.ChamberB.ToString(),
                C = ami?.ChamberC.ToString(),
                D = ami?.ChamberD.ToString(),

                Biobudent = 0,
                Endotoxin = 0,
                Particle = 0,
                QtyOutputSealing = 0,
                GaugeNoSealing = null,
                QtyOutputSterilize = 0,
                QtyFeaturesTest = 0,
                KeepAeration = Convert.ToInt32(dp.KeepStorage),
                Status = dataStatus
            };
        }).ToList();

        var batchEntity = new Domain.Entities.Batch
        {
            QRCode = $"Date@{request.SterilizeDate.ToShortDateString()}@BatchNo@{request.batchNo}",
            SterilizeDate = request.SterilizeDate,
            BatchNo = request.batchNo,
            Keep = keepDay,
            Status = dataStatus,

            BatchItems = batchItems
        };

        _batchRepositoryBase.Add(batchEntity);

        foreach (var item in dataPlans)
        {
            item.Status = dataStatus;
        }

        return Result.Success();
    }

    private static string GetDrawingNumber(string? itemCode)
        => string.IsNullOrEmpty(itemCode)
            ? string.Empty
            : itemCode.Split('-')[0];
}
