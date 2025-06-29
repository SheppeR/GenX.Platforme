using CommunityToolkit.Mvvm.ComponentModel;

namespace GenX.Network.Packets.FriendsDatas;

public class FriendDatas : ObservableRecipient
{
    private bool _isAccepted;
    public int ID { get; set; }
    public string? Pseudo { get; set; }
    public int Status { get; set; }

    public string? Avatar { get; set; }
    public string? Event { get; set; }

    public bool IsAccepted
    {
        get => _isAccepted;
        set => SetProperty(ref _isAccepted, value);
    }
}