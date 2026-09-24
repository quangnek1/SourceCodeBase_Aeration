using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.OutputAerationRoom;
public sealed record OutputAerationRoomCommand(int BatchId, IReadOnlyList<int> BatchItems, DateTimeOffset Date) : ICommand;
