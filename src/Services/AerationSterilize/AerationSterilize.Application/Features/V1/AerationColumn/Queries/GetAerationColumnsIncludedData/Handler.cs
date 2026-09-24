using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumnsIncludedData;
internal class GetAerationColumnsIncludedDataQueryHandler : IQueryHandler<GetAerationColumnsIncludedDataQuery,
    IReadOnlyList<AerationColumnIncludedDto>>
{
    private readonly IRepositoryBase<Domain.Entities.AerationColumn, int> _repository;
    private readonly IMapper _mapper;

    public GetAerationColumnsIncludedDataQueryHandler(
     IRepositoryBase<Domain.Entities.AerationColumn, int> repository,
     IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Result<IReadOnlyList<AerationColumnIncludedDto>>> Handle(GetAerationColumnsIncludedDataQuery request, CancellationToken cancellationToken)
    {
 
        var lamda =
             new Func<IQueryable<Domain.Entities.AerationColumn>,
                 IQueryable<Domain.Entities.AerationColumn>>(q =>
                 {
                     return q.Include(x => x.AerationPositions)
                             .ThenInclude(x => x.BatchInAerationPositions
                                 .Where(b =>
                                     b.Batch.Status == DataStatus.Aeration ||
                                     b.Batch.Status == DataStatus.AerationDone))
                             .ThenInclude(x => x.Batch);
                 });


        var query = _repository.FindAll(null,  false, lamda);

        var aerationColumnIncluded = await query.ToListAsync(cancellationToken);

        var result = _mapper.Map<IReadOnlyList<AerationColumnIncludedDto>>(aerationColumnIncluded);

        return Result.Success(result);
    }
}
