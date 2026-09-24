using AerationSterilize.Domain.Enums;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Commands.CreateAerationPosition;

internal sealed class CreateAerationPositionCommandHandler : ICommandHandler<CreateAerationPositionCommand, int>
{
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _repository;
    private readonly IMapper _mapper;

    public CreateAerationPositionCommandHandler(
        IRepositoryBase<Domain.Entities.AerationPosition, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<int>> Handle(CreateAerationPositionCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.AerationPosition>(request);

        _repository.Add(entity);

        return Result.Success(entity.Id);
    }
}
