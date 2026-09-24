using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.InputAerationRoom;
public sealed record InputAerationRoomCommand(int batchId, IReadOnlyList<int> positionIds, DateTimeOffset Date, string[] Location) : ICommand;
