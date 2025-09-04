using GenX.Network.Packets.FriendsDatas;

namespace GenX.Common.Collections;

public class FriendResultComparer : IComparer<FriendDatas>
{
    public int Compare(FriendDatas? x, FriendDatas? y)
    {
        if (x == null || y == null)
        {
            return x == null ? y == null ? 0 : -1 : 1;
        }

        if (x.IsAccepted != y.IsAccepted)
        {
            return x.IsAccepted ? 1 : -1;
        }

        var xStatusPriority = x.Status == 3 ? 0 : 1;
        var yStatusPriority = y.Status == 3 ? 0 : 1;
        if (xStatusPriority != yStatusPriority)
        {
            return xStatusPriority.CompareTo(yStatusPriority);
        }

        var xPseudo = x.Pseudo ?? string.Empty;
        var yPseudo = y.Pseudo ?? string.Empty;
        return string.Compare(xPseudo, yPseudo, StringComparison.OrdinalIgnoreCase);
    }
}