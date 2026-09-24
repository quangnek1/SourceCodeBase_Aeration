using AerationSterilize.Application.Abstractions;
using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Domain.Entities.Working;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Helpers;
using BoxingJobs = AerationSterilize.Domain.Entities.Working.BoxingJob;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanTag;
internal class ScanTagCommandHanlder : ICommandHandler<ScanTagCommand>
{
    private readonly ICurrentUserService _currentUserService;

    private readonly IRepositoryBase<Box, int> _boxRepository;
    private readonly IRepositoryBase<ItemTag, int> _itemTagRepository;
    private readonly IRepositoryBase<BoxingPosition, int> _boxingPositionRepository;
    private readonly IRepositoryBase<BatchItem, int> _batchItemRepository;
    private readonly IRepositoryBase<BoxingJobs, int> _boxingJobPositionRepository;
    private readonly IRepositoryBase<BoxingSession, int> _boxingSessionRepository;

    public ScanTagCommandHanlder(IRepositoryBase<Box, int> boxRepository, IRepositoryBase<ItemTag, int> itemTagRepository,
        IRepositoryBase<BoxingPosition, int> boxingPositionRepository,
        IRepositoryBase<BatchItem, int> batchItemRepository,
        IRepositoryBase<BoxingJobs, int> boxingJobPositionRepository,
        IRepositoryBase<BoxingSession, int> boxingSessionRepository,
         ICurrentUserService currentUserService)
    {
        _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
        _itemTagRepository = itemTagRepository ?? throw new ArgumentNullException(nameof(itemTagRepository));
        _boxingPositionRepository = boxingPositionRepository ?? throw new ArgumentNullException(nameof(boxingPositionRepository));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
        _boxingJobPositionRepository = boxingJobPositionRepository ?? throw new ArgumentNullException(nameof(boxingJobPositionRepository));
        _boxingSessionRepository = boxingSessionRepository ?? throw new ArgumentNullException(nameof(boxingSessionRepository));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<Result> Handle(ScanTagCommand request, CancellationToken cancellationToken)
    {
        if (!ItemTagQrCode.TryParse(request.QrCode, out var tag))
        {
            return Result.Failure(new Error("400", "Invalid tag QR Code."));
        }

        //Check if tag has already been scanned in the same box

        var itemTag = await _itemTagRepository.FindSingleAsync(x => x.SEQNo == tag.SeqNo, cancellationToken);

        if (itemTag != null)
        {
            return Result.Failure(new Error("400", $"Tag {itemTag.SEQNo} has already been scanned."));
        }

        // Check box đang đóng.
        var userId = _currentUserService.UserIdRequired;
        var session = await _boxingSessionRepository.FindSingleAsync(x => x.AppUserId == userId && x.IsActive, cancellationToken);
        if (session == null || session.CurrentBoxId == null)
        {
            return Result.Failure(new Error("400", "Please scan a Box first."));
        }

        var box = await _boxRepository.FindSingleAsync(x => x.Id == session.CurrentBoxId, cancellationToken);
        if (box == null)
        {
            return Result.Failure(new Error("400", "Current box not found."));
        }

        // Check itemtag trùng box không
        if (box.INT != tag.INT)
        {
            return Result.Failure(new Error("400", $"Tag {tag.INT} does not belong to box {box.INT}."));
        }

        var currentQty = await _itemTagRepository.FindAll(x => x.BoxId == box.Id).SumAsync(x => x.Qty, cancellationToken);

        if (currentQty + tag.Qty > box.Capacity)
        {
            return Result.Failure(new Error("400", "Box is full."));
        }

        if (currentQty + tag.Qty == box.Capacity)
        {
            box.Status = Shared.Emumerations.BoxStatus.Full;
        }

        _itemTagRepository.Add(Create(tag, box.Id));

        return Result.Success("Thành công.");

    }

    private ItemTag Create(ItemTagQrCode? tag, int boxId)
    {
        return new ItemTag
        {
            ItemcD = tag.ItemCD,
            SEQNo = tag.SeqNo,
            Qty = tag.Qty,
            INT = tag.INT,
            Ext1 = tag.Ext1,
            Ext2 = tag.Ext2,
            Type = tag.Type,
            Status = true,

            BoxId = boxId,
        };
    }
}
