using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using GenX.Client.Network;

namespace GenX.Client.ViewModels.User;

public partial class UserViewModel : ObservableRecipient, IRecipient<UserDatasMessage>
{
    [ObservableProperty] private string? _avatar;
    [ObservableProperty] private string? _event;
    [ObservableProperty] private string? _pseudo;
    [ObservableProperty] private int _status;

    public UserViewModel()
    {
        IsActive = true;
    }

    public void Receive(UserDatasMessage message)
    {
        Pseudo = message.response.Pseudo;
        Avatar = message.response.Avatar;
        Status = message.response.Status;
        Event = message.response.Event;
    }
}