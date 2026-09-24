using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumnsIncludedData;
using AerationSterilize.Infrastructure.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace AerationSterilize.Infrastructure.Services;

public class SignalRService : ISignalRServices
{
    private readonly IHubContext<AerationServiceHub> _hubContext;
    private readonly ISender _sender;

    public SignalRService(IHubContext<AerationServiceHub> hubContext, ISender sender)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public async Task PushAerationLayoutAsync()
    {
        var result = await _sender.Send(new GetAerationColumnsIncludedDataQuery());

        if (result.IsSuccess)
            await _hubContext.Clients.All.SendAsync("AerationUpdated", result.Value);
    }
}
