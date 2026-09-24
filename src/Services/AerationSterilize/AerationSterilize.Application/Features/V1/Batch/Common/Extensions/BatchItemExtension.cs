using System.Linq.Expressions;

namespace AerationSterilize.Application.Features.V1.Batch.Common.Extensions;

public static class BatchItemExtension
{
    public static Expression<Func<Domain.Entities.BatchItem, object>> GetSortExpression(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "internallot"             => x => x.InternalLot,
            "keepaeration"            => x => x.KeepAeration,
            "status"                  => x => x.Status,
            "inputaerationdate"       => x => x.InputAerationDate,
            "planoutputaerationdate"  => x => x.PlanOutputAerationDate,
            "actualoutputaerationdate"=> x => x.ActualOutputAerationDate,
            "inputboxingdate"         => x => x.InputBoxingDate,
            "outputboxingdate"        => x => x.OutputBoxingDate,
            "inputpackingdate"        => x => x.InputPackingDate,
            "outputpackingdate"       => x => x.OutputPackingDate,
            "batchno"                 => x => x.Batch.BatchNo,
            _                         => x => x.Id
        };
    }
}
