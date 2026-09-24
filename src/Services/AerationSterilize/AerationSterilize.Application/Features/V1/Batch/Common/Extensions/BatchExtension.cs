using System.Linq.Expressions;

namespace AerationSterilize.Application.Features.V1.Batch.Common.Extensions;

public static class BatchExtension
{
    /// <summary>
    /// hàm trả về tên thuộc tính của sản phẩm tương ứng với cột được truyền vào, nếu cột không hợp lệ sẽ trả về "Id"
    /// vào đó sẽ được sử dụng trong câu lệnh OrderBy hoặc OrderByDescending để sắp xếp kết quả truy vấn sản phẩm theo cột tương ứng.
    /// </summary>
    /// <param name="sortColumn"></param>
    /// <returns></returns>
    public static string GetSortBatchProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "batchno" => "BatchNo",
            _ => "Id"
        };
    /// <summary>
    /// hàm trả về biểu thức sắp xếp theo cột được truyền vào, nếu cột không hợp lệ sẽ sắp xếp theo Id
    /// vào đó sẽ được sử dụng trong câu lệnh OrderBy hoặc OrderByDescending để sắp xếp kết quả truy vấn sản phẩm theo cột tương ứng.
    /// vào đó sẽ giúp cho việc sắp xếp sản phẩm theo các thuộc tính khác nhau một cách linh hoạt và dễ dàng.
    /// </summary>
    /// <param name="sortColumn"></param>
    /// <returns></returns>
    public static Expression<Func<Domain.Entities.Batch, object>> GetSortExpression(string sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "batchno" => x => x.BatchNo,
            _ => x => x.Id
        };
    }
}
