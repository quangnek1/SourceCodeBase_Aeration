using System.ComponentModel;

namespace Shared.Emumerations;
public enum WorkTableStatus
{
    [Description("Trống")]
    Empty = 0,

    [Description("Đang tao tác")]
    InProgress = 1,

    [Description("Tạm dừng thao tác")]
    Paused = 2,
}
