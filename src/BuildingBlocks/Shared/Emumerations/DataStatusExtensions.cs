using System.ComponentModel;
using System.Reflection;

namespace Shared.Emumerations;
public static class DataStatusExtensions
{
    /// <summary>
    /// Lấy chuỗi mô tả từ thuộc tính [Description]
    /// </summary>
    public static string GetDescription(this DataStatus status)
    {
        FieldInfo? field = status.GetType().GetField(status.ToString());

        if (field == null)
            return status.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute != null ? attribute.Description : status.ToString();
    }
}
