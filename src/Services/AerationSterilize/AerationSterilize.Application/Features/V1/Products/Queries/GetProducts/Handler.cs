using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.Products.Common.Dtos;
using AerationSterilize.Application.Features.V1.Products.Common.Extensions;
using AerationSterilize.Domain.Entities;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.Extensions.Logging;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Products.Queries.GetProducts;
public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly IRepositoryBase<Product, Guid> _productRepository;
    private readonly IMapper _mapper;
    private readonly IProductSearchService _productSearchService;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    public GetProductsQueryHandler(
      IRepositoryBase<Product, Guid> productRepository,
      IMapper mapper,
      IProductSearchService productSearchService,
      ILogger<GetProductsQueryHandler> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _productSearchService = productSearchService ?? throw new ArgumentNullException(nameof(productSearchService));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<PagedResult<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
      if (!string.IsNullOrWhiteSpace(request.searchTerm) && _productSearchService.IsEnabled)
      {
        try
        {
          var searchResult = await _productSearchService.SearchAsync(
            request.searchTerm,
            request.sortColumn,
            request.SortOrder,
            request.PageIndex,
            request.PageSize,
            cancellationToken);

          return Result.Success(searchResult);
        }
        catch (Exception ex)
        {
          _logger.LogWarning(ex, "Elasticsearch product search failed. Falling back to SQL query.");
        }
      }

        var productsQuery = string.IsNullOrWhiteSpace(request.searchTerm)
          ? _productRepository.FindAll()
          : _productRepository.FindAll(x => x.Name.Contains(request.searchTerm) || x.Description.Contains(request.searchTerm));

        var sortExpression = ProductExtension.GetSortExpression(request.sortColumn);

        productsQuery = request.SortOrder == SortOrder.Descending
          ? productsQuery.OrderByDescending(sortExpression)
          : productsQuery.OrderBy(sortExpression);

        var pagedResult = await PagingExtensions.ToPagedResultAsync(
                        productsQuery,
                        request.PageIndex,
                        request.PageSize);

        var result = _mapper.Map<PagedResult<ProductDto>>(pagedResult);

        return Result.Success(result);
    }
}
