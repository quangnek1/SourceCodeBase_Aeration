using System.ComponentModel;

namespace Shared.Emumerations;
public enum DataStatus : byte
{
    [Description("Khởi tạo")]
    Initial = 0,

    [Description("Đã tạo")]
    Created = 1,

    [Description("Đã tái tạo")]
    ReCreated = 2,

    [Description("Đang sục khí")]
    Aeration = 3,

    [Description("Sục khí hoàn thành")]
    AerationDone = 4,

    [Description("Đang đóng hộp")]
    Boxing = 5,

    [Description("Đóng hộp hoàn thành")]
    BoxingDone = 6,

    [Description("Đang đóng gói")]
    Packing = 7,

    [Description("Đóng gói hoàn thành")]
    PackingDone = 8,

    [Description("Hoàn thành")]
    Done = 9
}
