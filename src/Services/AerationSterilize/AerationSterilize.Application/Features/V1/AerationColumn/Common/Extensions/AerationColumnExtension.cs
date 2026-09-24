using System.Linq.Expressions;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Common.Extensions;

public static class AerationColumnExtension
{
    public static Expression<Func<Domain.Entities.AerationColumn, object>> GetSortExpression(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "columnname" => x => x.ColumnName,
            "status" => x => x.Status,
            _ => x => x.Id
        };
    }
}
