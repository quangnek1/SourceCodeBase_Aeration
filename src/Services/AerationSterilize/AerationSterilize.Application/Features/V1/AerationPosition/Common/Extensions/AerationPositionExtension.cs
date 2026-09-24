using System.Linq.Expressions;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Common.Extensions;

public static class AerationPositionExtension
{
    public static Expression<Func<Domain.Entities.AerationPosition, object>> GetSortExpression(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "positioncode" => x => x.PositionCode,
            "status" => x => x.Status,
            "aerationcolumnid" => x => x.AerationColumnId,
            _ => x => x.Id
        };
    }
}
