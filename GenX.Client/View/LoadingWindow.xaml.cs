using System.Windows;
using GenX.Client.Network;
using GenX.Client.ViewModels.Windows;
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
        var friendsList = friendsDatas.FriendsData;


        for (var i = 0; i < 20; i++)
        {
            var friend = new FriendDatas
            {
                Pseudo = $"{i}crvgvtrtvhr",
                Status = 1, ID = i + 1, IsAccepted = true
            };
            friendsList?.Add(friend);
        }

        if (friendsList != null)
            foreach (var friend in friendsList)
                App.GetRequiredService<FriendsWindowViewModel>().Friends.Add(friend);

        App.GetRequiredService<LoadingWindow>().Close();
        App.GetRequiredService<MainWindow>().Show();
        App.GetRequiredService<FriendsWindow>().Show();
    }
}