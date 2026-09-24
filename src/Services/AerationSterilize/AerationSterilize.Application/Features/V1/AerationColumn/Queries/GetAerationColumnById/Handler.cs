using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumnById;

public class GetAerationColumnByIdQueryHandler : IQueryHandler<GetAerationColumnByIdQuery, AerationColumnDto>
{
    private readonly IRepositoryBase<Domain.Entities.AerationColumn, int> _repository;
    private readonly IMapper _mapper;

    public GetAerationColumnByIdQueryHandler(
        IRepositoryBase<Domain.Entities.AerationColumn, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<AerationColumnDto>> Handle(GetAerationColumnByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        var result = _mapper.Map<AerationColumnDto>(entity);

        return Result.Success(result);
    }
}
