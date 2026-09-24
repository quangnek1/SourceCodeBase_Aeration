using System.Linq.Expressions;
using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Domain.Entities.Working;
using AerationSterilize.Domain.Enums;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Emumerations;
using BoxingJobs = AerationSterilize.Domain.Entities.Working.BoxingJob;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBoxingPosition;
internal class ScanBoxingPositionCommandHandler : ICommandHandler<ScanBoxingPositionCommand, ScanBoxingPositionDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRepositoryBase<BoxingPosition, int> _boxingPositionRepository;
    private readonly IRepositoryBase<BoxingJobs, int> _boxingJobRepository;
    private readonly IRepositoryBase<BoxingSession, int> _boxingSessionRepository;

    public ScanBoxingPositionCommandHandler(ICurrentUserService currentUserService,
        IRepositoryBase<BoxingPosition, int> boxingPositionRepository,
        IRepositoryBase<BoxingJobs, int> boxingJobPositionRepository,
        IRepositoryBase<BoxingSession, int> boxingSessionRepository
        )
    {
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _boxingPositionRepository = boxingPositionRepository ?? throw new ArgumentNullException(nameof(boxingPositionRepository));
        _boxingJobRepository = boxingJobPositionRepository ?? throw new ArgumentNullException(nameof(boxingJobPositionRepository));
        _boxingSessionRepository = boxingSessionRepository ?? throw new ArgumentNullException(nameof(boxingSessionRepository));
    }

    public async Task<Result<ScanBoxingPositionDto>> Handle(ScanBoxingPositionCommand request, CancellationToken cancellationToken)
    {
        var position = await _boxingPositionRepository.FindSingleAsync(x => x.PositionCode == request.PositionCode, cancellationToken);

        if (position == null)
        {
            return Result.Failure<ScanBoxingPositionDto>(new Error("404", "Boxing position not found"));
        }

        if (position.Status == PositionStatus.Done)
        {
            return Result.Failure<ScanBoxingPositionDto>(new Error("400", $"Boxing position {position.PositionCode} is already done."));
        }

        var userId = _currentUserService.UserIdRequired;

        // 2. Lấy session hiện tại của User
        var session = await _boxingSessionRepository.FindSingleAsync(x => x.AppUserId == userId && x.IsActive, cancellationToken);

        if (session == null)
        {
            session = CreateBoxingSession(userId, position);
            _boxingSessionRepository.Add(session);
        }

        // 4. User chuyển sang BoxingPosition khác
        session.BoxingPositionId = position.Id;

        // Context cũ không còn áp dụng
        session.BoxingJobId = null;
        session.CurrentBoxId = null;

  //      _boxingSessionRepository.Update(session);

        // 5. Lấy các Job đang thực hiện tại Position
        var lamda = new Func<IQueryable<BoxingJobs>, IQueryable<BoxingJobs>>(q =>
        {
            return q.Include(x => x.BoxingPosition)
                    .Include(x => x.BatchItem);
        });

        var activeJobs = await _boxingJobRepository
            .FindAll(x => x.BoxingPositionId == position.Id && x.Status == BoxingJobStatus.InProgress,
            false,
            lamda)
            .ToListAsync(cancellationToken);

        // 6. Mapping
        var result = new ScanBoxingPositionDto
        {
            PositionId = position.Id,
            HasActiveJob = activeJobs.Count > 0,
            PositionCode = position.PositionCode,
            BoxingJobDtos = activeJobs.Select(x => new BoxingJobDto
            {
                Id = x.Id,
                BoxingPosition = x.BoxingPosition,
                BatchItem = x.BatchItem,
                TargetQty = x.TargetQty,
                Status = x.Status
            }).ToList()
        };

        return Result.Success(result);
    }

    private BoxingSession CreateBoxingSession(Guid userId, BoxingPosition boxingPosition)
    {
        return new BoxingSession
        {
            AppUserId = userId,
            BoxingPosition = boxingPosition,

            // Chưa scan Box
            BoxingJobId = null,
            CurrentBoxId = null,

            IsActive = true,
            StartedAt = DateTimeOffset.Now
        };
    }
}
