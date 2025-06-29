using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GenX.Client.View;

public partial class FriendsWindow
{
    public FriendsWindow()
    {
        InitializeComponent();
    }

    private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (!e.Handled)
        {
            e.Handled = true;

            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };

            var parent = ((Control)sender).Parent as UIElement;
            parent?.RaiseEvent(eventArg);
        }
    }
}