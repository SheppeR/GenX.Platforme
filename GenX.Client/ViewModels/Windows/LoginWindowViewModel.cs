using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using CodingSeb.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenX.Client.Network;
using GenX.Client.Properties;
using GenX.Client.View;
using GenX.Common.Extensions;
using GenX.Network.Packets.Login;
using Microsoft.Extensions.DependencyInjection;

namespace GenX.Client.ViewModels.Windows;

public partial class LoginWindowViewModel(IGenXClient client, IServiceProvider serviceProvider) : ObservableRecipient
{
    private string? _errorContent;
    private bool _isLoading;

    private bool _loginButtonEnabled;
    private string _loginText = !string.IsNullOrEmpty(Settings.Default.Login) ? Settings.Default.Login : string.Empty;

    private string? _passwordText = !string.IsNullOrEmpty(Settings.Default.Password)
        ? DecryptPassword(Settings.Default.Password)
        : string.Empty;

    private bool _saveLoginChecked = Settings.Default.SaveLogin;

    public string LoginText
    {
        get => _loginText;
        set
        {
            SetProperty(ref _loginText, value);
            CheckText();
        }
    }

    public string? PasswordText
    {
        get => _passwordText;
        set
        {
            SetProperty(ref _passwordText, value);
            CheckText();
        }
    }

    public bool SaveLoginChecked
    {
        get => _saveLoginChecked;
        set => SetProperty(ref _saveLoginChecked, value);
    }

    public bool LoginButtonEnabled
    {
        get => _loginButtonEnabled;
        set => SetProperty(ref _loginButtonEnabled, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string? ErrorContent
    {
        get => _errorContent;
        set => SetProperty(ref _errorContent, value);
    }

    [RelayCommand]
    public Task PasswordChanged(PasswordBox passwordBox)
    {
        PasswordText = passwordBox.Password;
        return Task.CompletedTask;
    }

    [RelayCommand]
    public async Task Login()
    {
        IsLoading = true;

        if (SaveLoginChecked)
            SaveCredentials(LoginText, PasswordText);
        else
            ClearCredentials();

        var rep = await client.SendAndReceive<LoginResponse>(new LoginRequest(LoginText, PasswordText.CreateMD5()));
        switch (rep.Result)
        {
            case LoginResult.LoginSuccess:
                serviceProvider.GetRequiredService<LoginWindow>()?.Close();
                serviceProvider.GetRequiredService<LoadingWindow>().Show();
                break;
            case LoginResult.LoginFail:
                ErrorContent = Loc.Tr("ErrorLoginFail");
                break;
            case LoginResult.AccountBanned:
                ErrorContent = Loc.Tr("ErrorLoginBanned");
                break;
            case LoginResult.AccountNotFound:
                ErrorContent = Loc.Tr("ErrorLoginNotFound");
                break;
            case LoginResult.LoginUnk:
                ErrorContent = Loc.Tr("ErrorLoginUnk");
                break;
        }
    }

    [RelayCommand]
    public Task Create()
    {
        ProcessStartInfo sInfo = new(new Uri(Settings.Default.CreateAccountUrl).AbsoluteUri) { UseShellExecute = true };

        _ = Process.Start(sInfo);
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task Forgot()
    {
        ProcessStartInfo sInfo = new(new Uri(Settings.Default.ForgotUrl).AbsoluteUri) { UseShellExecute = true };

        _ = Process.Start(sInfo);
        return Task.CompletedTask;
    }

    private void SaveCredentials(string login, string? password)
    {
        var encryptedPassword = EncryptPassword(password);

        Settings.Default.Login = login;
        Settings.Default.Password = encryptedPassword;
        Settings.Default.Save();
    }

    private void ClearCredentials()
    {
        Settings.Default.Login = string.Empty;
        Settings.Default.Password = string.Empty;
        Settings.Default.Save();
    }

    private string EncryptPassword(string? password)
    {
        if (string.IsNullOrEmpty(password))
            return string.Empty;

        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var encryptedBytes = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encryptedBytes);
    }

    private static string DecryptPassword(string encryptedPassword)
    {
        if (string.IsNullOrEmpty(encryptedPassword))
            return string.Empty;

        try
        {
            var encryptedBytes = Convert.FromBase64String(encryptedPassword);
            var decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    private void CheckText()
    {
        if (!string.IsNullOrEmpty(LoginText) && !string.IsNullOrEmpty(PasswordText))
        {
            if (PasswordText != null && LoginText.Length >= 6 && PasswordText.Length >= 6)
                LoginButtonEnabled = true;
            else
                LoginButtonEnabled = false;
        }
        else
        {
            LoginButtonEnabled = false;
        }
    }
}