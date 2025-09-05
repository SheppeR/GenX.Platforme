using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern.Controls;

namespace GenX.Client.ViewModels.Controls;

public partial class AlertViewModel : ObservableObject
{
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string message = string.Empty;
    [ObservableProperty] private InfoBarSeverity severity;
    [ObservableProperty] private bool isOpen = true;

    public AlertViewModel(string title, string message, InfoBarSeverity severity)
    {
        Title = title;
        Message = message;
        Severity = severity;

        _ = AutoCloseAsync();
    }

    private async Task AutoCloseAsync()
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            IsOpen = false;
        }
        catch
        {
            // ignored
        }
    }
}