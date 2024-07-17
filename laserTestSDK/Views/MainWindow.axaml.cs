using System;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;

namespace laserTestSDK.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        Environment.Exit(Environment.ExitCode);
        
    }
}