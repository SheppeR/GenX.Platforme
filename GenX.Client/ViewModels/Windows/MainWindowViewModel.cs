using CommunityToolkit.Mvvm.ComponentModel;
using GenX.Client.Network;
using GenX.Network.Packets.FriendsDatas;
using GenX.Network.Packets.UserDatas;

namespace GenX.Client.ViewModels.Windows;

public partial class MainWindowViewModel(IGenXClient client) : ObservableRecipient
{
    public void Send()
    {
        client.Send(new UserDatasRequest());
        client.Send(new FriendsDatasRequest());
    }
}