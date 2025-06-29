using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenX.Client.Network;
using GenX.Client.View;
using GenX.Network.Packets.Logout;
using Network.Enums;

namespace GenX.Client.ViewModels;

public partial class NotifyIconViewModel(IGenXClient client, MainWindow mainWindow, FriendsWindow friendsWindow)
    : ObservableRecipient
{
    private FriendsWindow _friendsWindow = friendsWindow;
    private MainWindow _mainWindow = mainWindow;

    [RelayCommand]
    public void ShowWindow()
    {
        if (!_mainWindow.IsVisible)
        {
            _mainWindow = new MainWindow();
            _mainWindow.Show();
        }
        else
        {
            if (_mainWindow.WindowState == WindowState.Minimized) _mainWindow.WindowState = WindowState.Normal;

            _mainWindow.Activate();
        }
    }

    [RelayCommand]
    public void ShowFriendsWindow()
    {
        if (!_friendsWindow.IsVisible)
        {
            _friendsWindow = new FriendsWindow();
            _friendsWindow.Show();
        }
        else
        {
            if (_friendsWindow.WindowState == WindowState.Minimized) _friendsWindow.WindowState = WindowState.Normal;

            _friendsWindow.Activate();
        }
    }

    [RelayCommand]
    public async Task ExitApplication()
    {
        var datas = await client.SendAndReceive<LogoutResponse>(new LogoutRequest());
        while (!datas.Success) await Task.Delay(10);
        await client.Disconnect(CloseReason.ClientClosed);
        Application.Current.Shutdown(0);
    }
}