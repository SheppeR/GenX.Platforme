using GenX.Client.ViewModels.Windows;

namespace GenX.Client.ViewModels;

public class ViewModelLocator
{
    public MainWindowViewModel MainWindow => App.GetRequiredService<MainWindowViewModel>();

    public LoginWindowViewModel LoginWindow => App.GetRequiredService<LoginWindowViewModel>();

    public LoadingWindowViewModel LoadingWindow => App.GetRequiredService<LoadingWindowViewModel>();
}