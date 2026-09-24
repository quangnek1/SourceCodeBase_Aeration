using Ardalis.SmartEnum;

namespace AerationSterilize.Domain.Enums;
public class BatchStatus : SmartEnum<BatchStatus>
{
    public BatchStatus(string name, int value) : base(name, value)
    {
    }

    public static readonly BatchStatus Initial = new(nameof(Initial), 0);
    public static readonly BatchStatus Aeration = new(nameof(Aeration), 1);
    public static readonly BatchStatus Boxing = new(nameof(Boxing), 2);
    public static readonly BatchStatus Packing = new(nameof(Packing), 3);
    public static readonly BatchStatus Done = new(nameof(Done), 4);

    public static implicit operator BatchStatus(string name) => FromName(name, true);
    public static implicit operator BatchStatus(int value) => FromValue(value);

    public static implicit operator string(BatchStatus status) => status.Name;
    public static implicit operator int(BatchStatus status) => status.Value;
}
