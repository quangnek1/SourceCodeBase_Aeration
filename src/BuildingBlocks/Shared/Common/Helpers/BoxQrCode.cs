namespace Shared.Common.Helpers;
public sealed class BoxQrCode
{
    public string BoxCode { get; }
    public int Capacity { get; }

    public BoxQrCode(string boxCode, int capacity)
    {
        BoxCode = boxCode;
        Capacity = capacity;
    }


    public static bool TryParse(string qrCode, out BoxQrCode? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(qrCode))
            return false;

        var parts = qrCode.Split('@');

        if (parts.Length != 2)
            return false;

        if (!int.TryParse(parts[1], out var capacity))
            return false;

        result = new BoxQrCode(parts[0], capacity);

        return true;
    }
}
