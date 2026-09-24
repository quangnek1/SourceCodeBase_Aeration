using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.CreateF6112;
public sealed record CreateF6112Command(
    DateTime SterilizeDate,
    string batchNo,
    IReadOnlyList<int>? listBatch) : ICommand;
