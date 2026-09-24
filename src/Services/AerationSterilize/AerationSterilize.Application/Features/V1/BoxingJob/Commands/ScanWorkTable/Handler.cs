using AerationSterilize.Application.Abstractions;
using AerationSterilize.Domain.Entities.Working;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanWorkTable;
internal class ScanWorkTableCommandHandler : ICommandHandler<ScanWorkTableCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRepositoryBase<WorkTableSession, int> _workTableSessionRepository;
    private readonly IRepositoryBase<WorkTable, int> _workTableRepository;

    public ScanWorkTableCommandHandler(ICurrentUserService currentUserService,
        IRepositoryBase<WorkTableSession, int> workTableSessionJobRepository,
        IRepositoryBase<WorkTable, int> workTableRepository)
    {
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _workTableSessionRepository = workTableSessionJobRepository ?? throw new ArgumentNullException(nameof(workTableSessionJobRepository));
        _workTableRepository = workTableRepository ?? throw new ArgumentNullException(nameof(workTableRepository));
    }

    public async Task<Result> Handle(ScanWorkTableCommand request, CancellationToken cancellationToken)
    {
        var workTable = await _workTableRepository.FindSingleAsync(x => x.Name == request.TableCode, cancellationToken);

        if (workTable == null)
        {
            return Result.Failure(new Error("404", $"Work table session with code {request.TableCode} not found."));
        }

        var currentUserId = _currentUserService.UserIdRequired;

        // Kiểm tra xem người dùng hiện tại có session đang hoạt động không
        var activeSession = await _workTableSessionRepository
            .FindSingleAsync(x => x.AppUserId == currentUserId && x.IsActive, cancellationToken);

        if (activeSession?.WorkTableId == workTable.Id)
            return Result.Success($"Đã xác nhận làm việc tại bàn: {request.TableCode}");

        // Nếu có session đang hoạt động, kết thúc nó trước khi tạo session mới

        if (activeSession != null)
        {
            activeSession.IsActive = false;
            activeSession.EndedAt = DateTimeOffset.UtcNow;

            _workTableSessionRepository.Update(activeSession); // nếu repository của mày cần gọi Update
        }

        // Tao moi mot session moi cho work table hien tai
        var session = WorkTableSession(currentUserId, workTable.Id);

        _workTableSessionRepository.Add(session);

        return Result.Success();
    }

    private WorkTableSession WorkTableSession(Guid currentUserId, int workTableId)
    {
        return new WorkTableSession
        {
            AppUserId = currentUserId,
            WorkTableId = workTableId,
            StartedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };
    }
}
