using System.ComponentModel;

namespace Shared.Emumerations;
public enum BoxingJobStatus
{
    [Description("Khởi tạo")]
    Created = 0,

    [Description("Đang tao tác")]
    InProgress = 1,

    [Description("Tạm dừng")]
    Paused = 2,

    [Description("Hoàn thành")]
    Completed = 3,

    [Description("Hủy bỏ")]
    Cancelled = 4
}
