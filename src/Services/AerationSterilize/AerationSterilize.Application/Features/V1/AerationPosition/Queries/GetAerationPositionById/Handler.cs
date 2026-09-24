using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Queries.GetAerationPositionById;

public class GetAerationPositionByIdQueryHandler : IQueryHandler<GetAerationPositionByIdQuery, AerationPositionDto>
{
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _repository;
    private readonly IMapper _mapper;

    public GetAerationPositionByIdQueryHandler(
        IRepositoryBase<Domain.Entities.AerationPosition, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<AerationPositionDto>> Handle(GetAerationPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        var result = _mapper.Map<AerationPositionDto>(entity);

        return Result.Success(result);
    }
}
