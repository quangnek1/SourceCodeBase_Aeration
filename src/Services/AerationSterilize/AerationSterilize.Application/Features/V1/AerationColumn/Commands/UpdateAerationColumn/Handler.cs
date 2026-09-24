using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Commands.UpdateAerationColumn;

internal sealed class UpdateAerationColumnCommandHandler : ICommandHandler<UpdateAerationColumnCommand, AerationColumnDto>
{
    private readonly IRepositoryBase<Domain.Entities.AerationColumn, int> _repository;
    private readonly IMapper _mapper;

    public UpdateAerationColumnCommandHandler(
        IRepositoryBase<Domain.Entities.AerationColumn, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<AerationColumnDto>> Handle(UpdateAerationColumnCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        entity = _mapper.Map(request, entity);

        _repository.Update(entity);

        var result = _mapper.Map<AerationColumnDto>(entity);

        return Result.Success(result);
    }
}
