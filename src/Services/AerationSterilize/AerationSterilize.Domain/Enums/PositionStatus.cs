using Ardalis.SmartEnum;

namespace AerationSterilize.Domain.Enums;
public class PositionStatus : SmartEnum<PositionStatus>
{
    public PositionStatus(string name, int value) : base(name, value)
    {
    }
    public static readonly PositionStatus Empty = new(nameof(Empty), 0);
    public static readonly PositionStatus InProgress = new(nameof(InProgress), 1);
    public static readonly PositionStatus Done = new(nameof(Done), 2);

    public static implicit operator PositionStatus(string name) => FromName(name, true);
    public static implicit operator PositionStatus(int value) => FromValue(value);

    public static implicit operator string(PositionStatus sortOrder) => sortOrder.Name;
    public static implicit operator int(PositionStatus sortOrder) => sortOrder.Value;
}
