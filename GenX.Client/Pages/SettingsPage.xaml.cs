using iNKORE.UI.WPF.Modern.Controls;

namespace GenX.Client.Pages;

public partial class SettingsPage
{
    public SettingsPage()
    {
        InitializeComponent();
        NavigationView.SelectedItem = NavigationView.MenuItems[0];
    }

    private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item)
        {
            var pagePath = $"/Pages/SettingsContent/{item.Tag}.xaml";

            if (!string.IsNullOrEmpty(pagePath))
            {
                ContentFrame.Source = new Uri(pagePath, UriKind.Relative);
            }
        }
    }
}