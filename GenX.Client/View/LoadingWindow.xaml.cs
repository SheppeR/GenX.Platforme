using System.Windows;
using GenX.Client.Network;
using GenX.Network.Packets.FriendsDatas;
using GenX.Network.Packets.UserDatas;

namespace GenX.Client.View;

public partial class LoadingWindow
{
    private readonly IGenXClient _client;

    public LoadingWindow(IGenXClient client)
    {
        _client = client;
        InitializeComponent();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        var userDatas = await _client.SendAndReceive<UserDatasResponse>(new UserDatasRequest());

        var friendsDatas = await _client.SendAndReceive<FriendsDatasResponse>(new FriendsDatasRequest());


        Console.WriteLine($"{friendsDatas} || {userDatas}");
    }
}