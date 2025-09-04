using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using GenX.Client.Properties;
using iNKORE.UI.WPF.Modern;

namespace GenX.Client.ViewModels.Content;

public partial class SettingsThemeContentViewModel : ObservableRecipient
{
    private Color _selectedAccentColor;
    private string? _selectedTheme;
    private ObservableCollection<string> _themes;

    public SettingsThemeContentViewModel()
    {
        _themes =
        [
            "Light",
            "Dark"
        ];

        switch (ThemeManager.Current.ApplicationTheme)
        {
            case ApplicationTheme.Light:
                _selectedTheme = "Light";
                break;
            case ApplicationTheme.Dark:
                _selectedTheme = "Dark";
                break;
        }
    }

    public string? SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            SetProperty(ref _selectedTheme, value);
            SelectTheme(value);
        }
    }

    public ObservableCollection<string> Themes
    {
        get => _themes;
        set => SetProperty(ref _themes, value);
    }

    public Color SelectedAccentColor
    {
        get => _selectedAccentColor;
        set
        {
            SetProperty(ref _selectedAccentColor, value);
            SelectAccentColor(value);
        }
    }

    private void SelectTheme(string? value)
    {
        switch (value)
        {
            case "Light":
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                Settings.Default.Theme = nameof(ApplicationTheme.Light);
                break;
            case "Dark":
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                Settings.Default.Theme = nameof(ApplicationTheme.Dark);
                break;
        }

        Settings.Default.Save();
    }

    private void SelectAccentColor(Color value)
    {
        ThemeManager.Current.AccentColor = value;
        Settings.Default.Accent = value.ToString();
        Settings.Default.Save();
    }
}