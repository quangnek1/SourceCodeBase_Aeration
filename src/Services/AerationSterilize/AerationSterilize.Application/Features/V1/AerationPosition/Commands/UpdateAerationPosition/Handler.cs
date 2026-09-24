using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Commands.UpdateAerationPosition;

internal sealed class UpdateAerationPositionCommandHandler : ICommandHandler<UpdateAerationPositionCommand, AerationPositionDto>
{
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _repository;
    private readonly IMapper _mapper;

    public UpdateAerationPositionCommandHandler(
        IRepositoryBase<Domain.Entities.AerationPosition, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<AerationPositionDto>> Handle(UpdateAerationPositionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        entity = _mapper.Map(request, entity);

        _repository.Update(entity);

        var result = _mapper.Map<AerationPositionDto>(entity);

        return Result.Success(result);
    }
}
