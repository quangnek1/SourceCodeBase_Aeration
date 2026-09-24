using System.Linq.Expressions;

namespace AerationSterilize.Application.Features.V1.Setting.Common.Extensions;

public static class SettingExtension
{
    public static Expression<Func<Domain.Entities.Setting, object>> GetSortExpression(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "plancag" => x => x.PlanCAG,
            "planptca" => x => x.PlanPTCA,
            "dataamiq411" => x => x.DataAmiQ411,
            _ => x => x.Id
        };
    }
}
