using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenX.Client.Network;
using GenX.Common.Collections;
using GenX.Network.Packets.FriendsDatas;

namespace GenX.Client.ViewModels.Windows;

public partial class FriendsWindowViewModel : ObservableRecipient, IRecipient<FriendsDatasMessage>
{
    private readonly IGenXClient _client;

    public FriendsWindowViewModel(IGenXClient client)
    {
        _client = client;
        IsActive = true;
    }

    public DispatcherSortedObservableCollection<FriendDatas> Friends { get; set; } = new(new FriendResultComparer());

    public void Receive(FriendsDatasMessage message)
    {
        if (message.response.FriendsData != null)
        {
            foreach (var friend in message.response.FriendsData)
            {
                Friends.Add(friend);
            }
        }
    }

    [RelayCommand]
    public async Task AcceptFriend(int id)
    {
        var rep = await _client.SendAndReceive<AcceptFriendResponse>(new AcceptFriendRequest(id));
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
        var rep = await _client.SendAndReceive<DenyFriendResponse>(new DenyFriendRequest(id));
        var friend = Friends.FirstOrDefault(f => f.ID.Equals(rep.FriendID));

        if (friend != null)
        {
            Friends.Remove(friend);
        }
    }
}