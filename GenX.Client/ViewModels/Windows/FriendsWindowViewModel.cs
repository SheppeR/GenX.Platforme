using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenX.Client.Network;
using GenX.Common.Collections;
using GenX.Network.Packets.FriendsDatas;

namespace GenX.Client.ViewModels.Windows;

public partial class FriendsWindowViewModel(IGenXClient client) : ObservableRecipient
{
    public SortedObservableCollection<FriendDatas> Friends { get; set; } = new(new FriendResultComparer());

    [RelayCommand]
    public async Task AcceptFriend(int id)
    {
        var rep = await client.SendAndReceive<AcceptFriendResponse>(new AcceptFriendRequest(id));
        var friend = Friends.FirstOrDefault(f => f.ID.Equals(rep.FriendID));

        if (friend != null)
        {
            friend.IsAccepted = true;
            Friends.Sort();
        }
    }

    [RelayCommand]
    public async Task DenyFriend(int id)
    {
        var rep = await client.SendAndReceive<DenyFriendResponse>(new DenyFriendRequest(id));
        var friend = Friends.FirstOrDefault(f => f.ID.Equals(rep.FriendID));

        if (friend != null) Friends.Remove(friend);
    }
}