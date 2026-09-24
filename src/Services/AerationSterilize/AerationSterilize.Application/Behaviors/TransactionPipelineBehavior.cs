using System.Transactions;
using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.DataPlan.Events;
using Contracts.Common.Repositories;
using MediatR;

namespace AerationSterilize.Application.Behaviors;
public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly ISignalRServices _signalRServices;

    public TransactionPipelineBehavior(IUnitOfWork unitOfWork, IPublisher publisher, ISignalRServices signalRServices)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _signalRServices = signalRServices ?? throw new ArgumentNullException(nameof(signalRServices));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsCommand()) // In case TRequest is QueryRequest just ignore
            return await next();

        TResponse response;
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Complete();
        }

        // 2. TRANSACTION THÀNH CÔNG -> Kiểm tra xem có phải lệnh SyncData không để bắn Event dọn cache
        if (typeof(TRequest).Name.StartsWith("SyncData") || typeof(TRequest).Name.StartsWith("CreateF6112"))
        {
            // Inject thêm IMediator vào constructor của Behavior này
            await _publisher.Publish(new DataPlanSyncedEvent(Guid.NewGuid()), cancellationToken);
        }

        if (typeof(TRequest).Name.StartsWith("InputAerationRoom") || 
            typeof(TRequest).Name.StartsWith("OutputAerationRoom") ||
            typeof(TRequest).Name.StartsWith("UpdateBatchStatus"))
        {
            await _signalRServices.PushAerationLayoutAsync();
        }

        return response;
    }

    private bool IsCommand()
      => typeof(TRequest).Name.EndsWith("Command");
}
