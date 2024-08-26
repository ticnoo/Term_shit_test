using appTest.ViewModels;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using System;
using System.Globalization;

namespace appTest.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        DataContextChanged += MainView_DataContextChanged;
    }

    private void MainView_DataContextChanged(object sender, EventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            TabControl tabControl = this.FindControl<TabControl>("MainTabs");

            viewModel.MainTabs = this.MainTabs; //tabControl;

            ScrollViewer scrollViewer = this.FindControl<ScrollViewer>("MainConsoleScroll");
            viewModel.MainConsoleScroll = scrollViewer;

            scrollViewer = this.FindControl<ScrollViewer>("MessengerConsoleScroll");
            viewModel.MessengerConsoleScroll = scrollViewer;

            scrollViewer = this.FindControl<ScrollViewer>("historyMainConsoleScroll");
            viewModel.HistoryMainConsoleScroll = scrollViewer;

            //viewModel.buttonConnect(null);
        }
    }

    public string textOnMainWindow { get; set; } = "text on main window";
}
