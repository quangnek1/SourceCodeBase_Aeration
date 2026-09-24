using Ardalis.SmartEnum;

namespace AerationSterilize.Domain.Enums;
public class AerationStatus : SmartEnum<AerationStatus>
{
    public AerationStatus(string name, int value) : base(name, value)
    {

    }
    public static readonly AerationStatus Empty = new(nameof(Empty), 0);
    public static readonly AerationStatus InProgress = new(nameof(InProgress), 1);
    public static readonly AerationStatus Done = new(nameof(Done), 2);

    public static implicit operator AerationStatus(string name) => FromName(name, true);
    public static implicit operator AerationStatus(int value) => FromValue(value);

    public static implicit operator string(AerationStatus sortOrder) => sortOrder.Name;
    public static implicit operator int(AerationStatus sortOrder) => sortOrder.Value;
}

