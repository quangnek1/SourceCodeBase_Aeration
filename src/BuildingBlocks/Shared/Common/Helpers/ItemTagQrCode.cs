using System.Globalization;
using System.Text.RegularExpressions;

namespace Shared.Common.Helpers;

public sealed class ItemTagQrCode
{
    public string SeqNo { get; }

    public string ItemCD { get; }

    public string INT { get; }

    public int Type { get; }
    public int Qty { get; }

    public string Ext1 { get; }

    public string Ext2 { get; }

    private ItemTagQrCode(
        string seqNo,
        string itemCD,
        string @int,
        int qty,
        string ext1,
        string ext2,
        int type)
    {
        SeqNo = seqNo;
        ItemCD = itemCD;
        INT = @int;
        Qty = qty;
        Ext1 = ext1;
        Ext2 = ext2;
        Type = type;
    }

    public static bool TryParse(string qrCode, out ItemTagQrCode? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(qrCode))
            return false;

        var values = Regex.Matches(qrCode, @"\(D@(\d+)\)([^\(]+)")
            .ToDictionary(
                x => x.Groups[1].Value,
                x => x.Groups[2].Value.Trim());

        if (!values.TryGetValue("02", out var seqNo))
            return false;



        if (!values.TryGetValue("12", out var itemCd))
            return false;

        if (!values.TryGetValue("30", out var lotNo))
            return false;

        if (!values.TryGetValue("06", out var qtyString))
            return false;

        if (!double.TryParse(qtyString, NumberStyles.Any, CultureInfo.InvariantCulture, out var qty))
            return false;

        if (!int.TryParse("01", out var type))
            return false;

        values.TryGetValue("04", out var ext1);
        values.TryGetValue("10", out var ext2);

        result = new ItemTagQrCode(
            seqNo,
            itemCd,
            lotNo,
            (int)qty,
            ext1 ?? string.Empty,
            ext2 ?? string.Empty,
            type);

        return true;
    }
}
