using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace GenX.Common.Helpers.Controls;

[SuppressUnmanagedCodeSecurity]
public static class ConsoleManager
{
    private const string Kernel32_DllName = "kernel32.dll";

    public static bool HasConsole => GetConsoleWindow() != IntPtr.Zero;

    [DllImport(Kernel32_DllName)]
    private static extern bool AllocConsole();

    [DllImport(Kernel32_DllName)]
    private static extern bool FreeConsole();

    [DllImport(Kernel32_DllName)]
    private static extern IntPtr GetConsoleWindow();

    public static void Show()
    {
        if (!HasConsole) AllocConsole();
    }

    public static void Hide()
    {
        if (HasConsole)
        {
            SetOutAndErrorNull();
            FreeConsole();
        }
    }

    public static void Toggle()
    {
        if (HasConsole)
            Hide();
        else
            Show();
    }

    private static void SetOutAndErrorNull()
    {
        Console.SetOut(TextWriter.Null);
        Console.SetError(TextWriter.Null);
    }
}