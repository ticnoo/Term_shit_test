using appTest.ViewModels;
using Avalonia.Controls;
using System;

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
            ScrollViewer scrollViewer = this.FindControl<ScrollViewer>("mainConcoleScroll");
            viewModel.MainConcoleScroll = scrollViewer;
        }
    }
}
