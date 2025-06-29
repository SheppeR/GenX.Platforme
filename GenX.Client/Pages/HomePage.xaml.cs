using System.Windows;
using GenX.Client.ViewModels.Windows;
using GenX.Network.Packets.FriendsDatas;

namespace GenX.Client.Pages;

public partial class HomePage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        App.GetRequiredService<FriendsWindowViewModel>().Friends.Add(new FriendDatas
            { ID = 35, IsAccepted = true, Pseudo = "coucou", Status = 3, Event = "INGAME" });
    }
}