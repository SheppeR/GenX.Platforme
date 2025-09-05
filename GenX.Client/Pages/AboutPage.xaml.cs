using System.Diagnostics;
using System.Windows.Navigation;

namespace GenX.Client.Pages;

public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = e.Uri.AbsoluteUri,
            UseShellExecute = true
        });
        e.Handled = true;
    }
}