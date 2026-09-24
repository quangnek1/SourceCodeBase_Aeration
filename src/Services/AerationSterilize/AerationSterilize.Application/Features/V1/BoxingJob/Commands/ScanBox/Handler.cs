using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Domain.Entities.Working;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Helpers;
using BoxingJobs = AerationSterilize.Domain.Entities.Working.BoxingJob;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBox;
internal class ScanBoxCommandHandler : ICommandHandler<ScanBoxCommand, ScanBoxDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IRepositoryBase<Box, int> _boxRepository;
    private readonly IRepositoryBase<ItemTag, int> _itemTagRepository;
    private readonly IRepositoryBase<BoxingJobs, int> _boxingJobPositionRepository;
    private readonly IRepositoryBase<BatchItem, int> _batchItemRepository;
    private readonly IRepositoryBase<BoxingSession, int> _boxingSessionRepository;

    public ScanBoxCommandHandler(IRepositoryBase<Box, int> boxRepository, IRepositoryBase<ItemTag, int> itemTagRepository,
        IRepositoryBase<BoxingJobs, int> boxingJobPositionRepository, IRepositoryBase<BatchItem, int> batchItemRepository,
        IRepositoryBase<BoxingSession, int> boxingSessionRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
        _itemTagRepository = itemTagRepository ?? throw new ArgumentNullException(nameof(itemTagRepository));
        _boxingJobPositionRepository = boxingJobPositionRepository ?? throw new ArgumentNullException(nameof(boxingJobPositionRepository));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
        _boxingSessionRepository = boxingSessionRepository ?? throw new ArgumentNullException(nameof(boxingSessionRepository));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<ScanBoxDto>> Handle(ScanBoxCommand request, CancellationToken cancellationToken)
    {
        // Parse QR
        if (!ItemTagQrCode.TryParse(request.BoxCode, out var tag))
        {
            return Result.Failure<ScanBoxDto>(new Error("400", "Invalid box QR Code."));
        }
        var userId = _currentUserService.UserIdRequired;
        var tagLotNo = tag.INT;

        // Check Session hiện tại Đã scan Boxing Position chưa. Nếu chưa scan thì báo lỗi.
        // 1. Lấy session hiện tại
        var session = await _boxingSessionRepository
            .FindSingleAsync(x => x.AppUserId == userId && x.IsActive && x.BoxingPositionId != null,
            cancellationToken);
        if (session == null)
        {
            return Result.Failure<ScanBoxDto>(new Error("400", "Please scan Boxing Position first."));
        }
        else if (session.BoxingJobId != null || session.CurrentBoxId != null)
        {
            // Check xem thùng đang đóng của lot hiện tại. Nếu thùng đang đóng chưa đầy thì không được mở thùng mới.
            var currentBox = await _boxRepository.FindSingleAsync(x => x.Id == session.CurrentBoxId, cancellationToken);
            var currentJob = await _boxingJobPositionRepository.FindSingleAsync(x => x.Id == session.BoxingJobId, cancellationToken);

            // So sánh box hiện tại với box scan. Cùng Lot thì so sánh số lượng. Khác lot thì chuyển sang box mới.
            if (currentBox.INT == tagLotNo)
            {
                // Cùng Lot. Giờ so sánh SEQ đã scan chưa
                var box = await _boxRepository.FindSingleAsync(x => x.SEQ == tag.SeqNo && x.INT == tagLotNo, cancellationToken);
                if (box != null)
                {
                    var currentQty = await _itemTagRepository.FindAll(x => x.BoxId == currentBox.Id).SumAsync(x => (int?)x.Qty, cancellationToken);
                    if (currentQty < currentBox.Capacity)
                    {
                        return Result.Failure<ScanBoxDto>(new Error("400", $"Current box {currentBox.BoxCode} - {currentQty}/{currentBox.Capacity} is not full yet."));
                    }
                    if (currentQty == currentBox.Capacity)
                    {
                        return Result.Failure<ScanBoxDto>(new Error("400", $"Box {request.BoxCode} is full."));
                    }
                }
                else
                {
                    // Khồng tồn tại. Kiểm tra xem thùng đã đóng chưa.
                    // Chỗ này là cùng Lot nhưng thùng khác.
                    // Kiểm tra xem thùng hiện tại đã đầy chưa
                    var currentQty = await _itemTagRepository.FindAll(x => x.BoxId == currentBox.Id).SumAsync(x => (int?)x.Qty, cancellationToken);
                    if (currentQty < currentBox.Capacity)
                    {
                        return Result.Failure<ScanBoxDto>(new Error("400", $"Cannot scan new box. Current box {currentBox.BoxCode} - {currentQty}/{currentBox.Capacity} is not full yet."));
                    }
                    if (currentQty == currentBox.Capacity)
                    {
                        return Result.Failure<ScanBoxDto>(new Error("400", $"Box {request.BoxCode} is full."));
                    }

                    var boxNew = CreateBox(tag, session.BoxingPositionId, currentJob);
                    _boxRepository.Add(boxNew);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    session.CurrentBox = boxNew;

                    var result = new ScanBoxDto
                    {
                        Id = boxNew.Id,
                        BoxCode = boxNew.BoxCode,
                        CurrentQty = 0,
                        Capacity = boxNew.Capacity,
                    };
                    return Result.Success(result);
                }

            }
            else
            {
                // Khác Lot thì chuyển Lot mới.
                // Kiểm tra xem đã tạo BoxingJob cho Lot mới chưa. Nếu chưa thì tạo mới. Nếu đã có thì lấy job đó ra.
                // 1. Check xem ItemTag trong DB.
                var batchItem = await _batchItemRepository.FindSingleAsync(x => x.InternalLot == tagLotNo, cancellationToken);
                if (batchItem is null || batchItem.Status != Shared.Emumerations.DataStatus.Boxing)
                {
                    return Result.Failure<ScanBoxDto>(batchItem is null
                        ? new Error("404", $"Batch item with lot {tagLotNo} not found.")
                        : new Error("400", $"Batch item with lot {tagLotNo} is not in boxing status."));
                }

                var boxingJob = await _boxingJobPositionRepository.FindSingleAsync(x => x.BatchItemId == batchItem.Id, cancellationToken);
                if (boxingJob == null)
                {
                    // Chưa có job. tạo job mới.
                    boxingJob = CreateBoxingJob(session.BoxingPositionId, batchItem);
                    _boxingJobPositionRepository.Add(boxingJob);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                // update session BoxingJobId và CurrentBoxId
                session.BoxingJob = boxingJob;

                var box = CreateBox(tag, session.BoxingPositionId, boxingJob);
                _boxRepository.Add(box);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                session.CurrentBox = box;

                var result = new ScanBoxDto
                {
                    Id = box.Id,
                    BoxCode = box.BoxCode,
                    CurrentQty = 0,
                    Capacity = box.Capacity,
                };
                return Result.Success(result);
            }
        }
        else
        {
            // Chỗ này là scan vị trí mới. Nên jobId và boxId null
            // Check thùng hiện tại đang đóng cho Lot nào.
            // 1. Check xem ItemTag trong DB.
            var batchItem = await _batchItemRepository.FindSingleAsync(x => x.InternalLot == tagLotNo, cancellationToken);
            if (batchItem is null || batchItem.Status != Shared.Emumerations.DataStatus.Boxing)
            {
                return Result.Failure<ScanBoxDto>(batchItem is null
                    ? new Error("404", $"Batch item with lot {tagLotNo} not found.")
                    : new Error("400", $"Batch item with lot {tagLotNo} is not in boxing status."));
            }
            var boxingJob = await _boxingJobPositionRepository
                .FindSingleAsync(x => x.BatchItemId == batchItem.Id && x.BoxingPositionId == session.BoxingPositionId, cancellationToken);
            if (boxingJob == null)
            {
                // Chưa có job. tạo job mới.
                boxingJob = CreateBoxingJob(session.BoxingPositionId, batchItem);
                _boxingJobPositionRepository.Add(boxingJob);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            // update session BoxingJobId và CurrentBoxId
            session.BoxingJob = boxingJob;

            // Check box đã tồn tại chưa.
            // CHeck xem lot/job này đang đóng ở thùng nào.
            var boxCheck = await _boxRepository.FindAll(
                x => x.INT == tagLotNo &&
                x.BoxingJobId == boxingJob.Id &&
                x.Status == Shared.Emumerations.BoxStatus.Open &&
                x.BoxingPositionId == session.BoxingPositionId
                ).ToListAsync(cancellationToken);

            if (boxCheck.Count() > 0)
            {
                var box = boxCheck.First();
                session.CurrentBox = box;

                var currentQty = await _itemTagRepository.FindAll(x => x.BoxId == box.Id).SumAsync(x => (int?)x.Qty, cancellationToken) ?? 0;
                if (currentQty >= box.Capacity)
                {
                    return Result.Failure<ScanBoxDto>(new Error("400", $"Box {request.BoxCode} is full."));
                }
                var result = new ScanBoxDto
                {
                    Id = box.Id,
                    BoxCode = box.BoxCode,
                    CurrentQty = currentQty,
                    Capacity = box.Capacity,
                };
                return Result.Success(result);
            }
            else
            {
                var box = await _boxRepository.FindSingleAsync(x => x.SEQ == tag.SeqNo && x.INT == tagLotNo, cancellationToken);
                if (box == null)
                {
                    box = CreateBox(tag, session.BoxingPositionId, boxingJob);
                    _boxRepository.Add(box);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                var currentQty = await _itemTagRepository.FindAll(x => x.BoxId == box.Id).SumAsync(x => (int?)x.Qty, cancellationToken) ?? 0;
                if (currentQty >= box.Capacity)
                {
                    return Result.Failure<ScanBoxDto>(new Error("400", $"Box {request.BoxCode} is full."));
                }

                session.CurrentBox = box;

                var result = new ScanBoxDto
                {
                    Id = box.Id,
                    BoxCode = box.BoxCode,
                    CurrentQty = currentQty,
                    Capacity = box.Capacity,
                };
                return Result.Success(result);
            }


        }
        return Result.Failure<ScanBoxDto>(new Error("400", "Unhandled case."));




    }

    private BoxingJobs CreateBoxingJob(int boxingPositionId, BatchItem batch)
    {
        return new BoxingJobs
        {
            BoxingPositionId = boxingPositionId,
            BatchItem = batch,
            TargetQty = batch.QtyInput,
            Status = Shared.Emumerations.BoxingJobStatus.InProgress,
            StartedAt = DateTimeOffset.Now,
        };
    }

    private Box CreateBox(ItemTagQrCode tag, int boxingPositionId, BoxingJobs boxingJobs)
    {
        return new Box
        {
            BoxCode = tag.SeqNo,
            BatchItem = boxingJobs.BatchItem,
            Capacity = tag.Qty,
            INT = tag.INT,
            SEQ = tag.SeqNo,
            Status = Shared.Emumerations.BoxStatus.Open,

            BoxingPositionId = boxingPositionId,
            BoxingJob = boxingJobs
        };
    }
}
