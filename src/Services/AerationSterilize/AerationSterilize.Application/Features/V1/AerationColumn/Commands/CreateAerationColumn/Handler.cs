using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Commands.CreateAerationColumn;

internal sealed class CreateAerationColumnCommandHandler : ICommandHandler<CreateAerationColumnCommand, int>
{
    private readonly IRepositoryBase<Domain.Entities.AerationColumn, int> _repository;
    private readonly IMapper _mapper;

    public CreateAerationColumnCommandHandler(
        IRepositoryBase<Domain.Entities.AerationColumn, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<int>> Handle(CreateAerationColumnCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.AerationColumn>(request);

        _repository.Add(entity);

        return Result.Success(entity.Id);
    }
}
