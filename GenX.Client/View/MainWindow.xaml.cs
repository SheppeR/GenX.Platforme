using System.IO;
using System.Windows.Navigation;
using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Controls.Primitives;

namespace GenX.Client.View;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        NavigationView.SelectedItem = NavigationView.MenuItems[0];
    }

    private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item)
        {
            var pagePath = $"/Pages/{item.Tag}.xaml";

            if (!string.IsNullOrEmpty(pagePath)) ContentFrame.Source = new Uri(pagePath, UriKind.Relative);
        }
    }

    private void OnTitleBarBackRequested(object sender, BackRequestedEventArgs e)
    {
        if (ContentFrame.CanGoBack)
        {
            ContentFrame.GoBack();
            e.Handled = true;
        }
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (e.Uri == null)
            return;

        var pageName = Path.GetFileNameWithoutExtension(e.Uri.ToString());
        foreach (var menuItem in NavigationView.MenuItems.OfType<NavigationViewItem>())
            if ((string)menuItem.Tag == pageName)
            {
                NavigationView.SelectedItem = menuItem;
                break;
            }

        foreach (var footerItem in NavigationView.FooterMenuItems.OfType<NavigationViewItem>())
            if ((string)footerItem.Tag == pageName)
            {
                NavigationView.SelectedItem = footerItem;
                break;
            }
    }
}