using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using GenX.Client.Network;

namespace GenX.Client.ViewModels.User;

public class UserViewModel : ObservableRecipient, IRecipient<UserDatasMessage>
{
    private string? _avatar;
    private string? _event;
    private string? _pseudo;
    private int _status;

    public UserViewModel()
    {
        IsActive = true;
    }

    public string? Pseudo
    {
        get => _pseudo;
        set => SetProperty(ref _pseudo, value);
    }

    public string? Avatar
    {
        get => _avatar;
        set => SetProperty(ref _avatar, value);
    }

    public int Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public string? Event
    {
        get => _event;
        set => SetProperty(ref _event, value);
    }

    public void Receive(UserDatasMessage message)
    {
        _pseudo = message.response.Pseudo;
        _avatar = message.response.Avatar;
        _status = message.response.Status;
        _event = message.response.Event;
    }
}