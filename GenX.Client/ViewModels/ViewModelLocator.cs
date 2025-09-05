using GenX.Client.ViewModels.Pages;
using GenX.Client.ViewModels.User;
using GenX.Client.ViewModels.Windows;

namespace GenX.Client.ViewModels;

public class ViewModelLocator
{
    public MainWindowViewModel MainWindow => App.GetRequiredService<MainWindowViewModel>();

    public LoginWindowViewModel LoginWindow => App.GetRequiredService<LoginWindowViewModel>();

    public FriendsWindowViewModel FriendsWindow => App.GetRequiredService<FriendsWindowViewModel>();

    public NotifyIconViewModel NotifyIcon => App.GetRequiredService<NotifyIconViewModel>();

    public UserViewModel User => App.GetRequiredService<UserViewModel>();

    public SettingsPageViewModel SettingsPage => App.GetRequiredService<SettingsPageViewModel>();
}