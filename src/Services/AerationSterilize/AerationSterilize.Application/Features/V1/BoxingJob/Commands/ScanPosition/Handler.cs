using AerationSterilize.Application.Abstractions;
using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Entities.Working;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanPosition;
internal class ScanPositionCommandHandler : ICommandHandler<ScanPositionCommand>
{
    private readonly IRepositoryBase<PackingPosition, int> _packingPositionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRepositoryBase<WorkTableSession, int> _sessionRepository;
    private readonly IRepositoryBase<WorkTableAssignment, int> _assignmentRepository;

    public ScanPositionCommandHandler(IRepositoryBase<PackingPosition, int> packingPositionRepository,
        ICurrentUserService currentUserService,
        IRepositoryBase<WorkTableSession, int> sessionRepository,
        IRepositoryBase<WorkTableAssignment, int> assignmentRepository)
    {
        _packingPositionRepository = packingPositionRepository ?? throw new ArgumentNullException(nameof(packingPositionRepository));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
    }

    public async Task<Result> Handle(ScanPositionCommand request, CancellationToken cancellationToken)
    {
        var position = await _packingPositionRepository
            .FindSingleAsync(x => x.PositionCode == request.PositionCode, cancellationToken);

        if (position == null)
            return Result.Failure(new Error("404", "Position not found"));

        var userId = _currentUserService.UserIdRequired;


        var session = await _sessionRepository.FindSingleAsync(x => x.AppUserId == userId && x.IsActive, cancellationToken);

        if (session == null)
        {
            return Result.Failure(new Error("400", "Please scan work table first."));
        }

        var assignment = await _assignmentRepository.FindSingleAsync(x => x.WorkTableId == session.WorkTableId && x.IsActive, cancellationToken);

        if (assignment != null && assignment.PackingPositionId == position.Id)
        {
            return Result.Success($"Đã vào vị trí: {request.PositionCode}");
        }

        if (assignment != null)
        {
            var currentPosition = await _packingPositionRepository.FindSingleAsync(x => x.Id == assignment.PackingPositionId, cancellationToken);
            return Result.Failure(new Error("409", $"Work table is working at: {currentPosition.PositionCode}."));
        }

        var newAssignment = new WorkTableAssignment
        {
            WorkTableId = session.WorkTableId, 
            PackingPositionId = position.Id,
            StartedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };

        _assignmentRepository.Add(newAssignment);

        return Result.Success($"Đã vào vị trí: {request.PositionCode}");
    }


}
