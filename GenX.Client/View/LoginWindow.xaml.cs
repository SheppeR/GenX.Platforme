using System.Windows;
using GenX.Client.ViewModels.Windows;
using iNKORE.UI.WPF.Modern.Controls;

namespace GenX.Client.View;

public partial class LoginWindow
{
    private readonly LoginWindowViewModel _model;

    public LoginWindow(LoginWindowViewModel model)
    {
        InitializeComponent();
        _model = model;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(_model.PasswordText))
        {
            Password.Password = _model.PasswordText;
        }
    }

    private void OnInfoBarClosing(InfoBar sender, InfoBarClosingEventArgs args)
    {
        _model.ErrorContent = string.Empty;
    }
}