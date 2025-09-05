using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using GenX.Client.Network;
using GenX.Client.ViewModels.Controls;
using GenX.Network.Packets.FriendsDatas;
using GenX.Network.Packets.UserDatas;
using iNKORE.UI.WPF.Modern.Controls;

namespace GenX.Client.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableRecipient, IRecipient<AlertMessage>
{
    [ObservableProperty] private string? _clockTime = DateTime.Now.ToString("HH:mm").ToUpper();
    [ObservableProperty] private string? _dateDay = DateTime.Now.ToString("dddd").ToUpper();
    [ObservableProperty] private string? _dateMonth = DateTime.Now.ToString("MMMM").ToUpper();
    [ObservableProperty] private string? _dateNumber = DateTime.Today.Day.ToString().ToUpper();
    [ObservableProperty] private string? _title;
    [ObservableProperty] private string? _message;
    [ObservableProperty] private InfoBarSeverity _severity;
    [ObservableProperty] private bool _isOpen;

    private readonly IGenXClient _client;

    public ObservableCollection<AlertViewModel> Alerts { get; } = new();

    public MainWindowViewModel(IGenXClient client)
    {
        IsActive = true;
        _client = client;
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timer.Tick += OnTick;
        timer.Start();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        DateDay = DateTime.Now.ToString("dddd").ToUpper();
        DateMonth = DateTime.Now.ToString("MMMM").ToUpper();
        DateNumber = DateTime.Today.Day.ToString().ToUpper();
        ClockTime = DateTime.Now.ToString("HH:mm").ToUpper();
    }

    public void Send()
    {
        _client.Send(new UserDatasRequest());
        _client.Send(new FriendsDatasRequest());
    }

    public void Receive(AlertMessage message)
    {
        var newAlert = new AlertViewModel(message.title, message.message, message.severity);
        Alerts.Add(newAlert);

        newAlert.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AlertViewModel.IsOpen) && !newAlert.IsOpen)
            {
                Alerts.Remove(newAlert);
            }
        };
    }
}