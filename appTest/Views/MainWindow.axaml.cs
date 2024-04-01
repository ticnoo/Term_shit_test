using Avalonia.Controls;
using appTest.ViewModels;

namespace appTest.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        //ScrollViewer scrollViewer = this.FindControl<ScrollViewer>("mainConcoleScroll");
        //MainViewModel viewModel = (MainViewModel)this.DataContext;
        //viewModel.MainConcoleScroll = scrollViewer;
    }
}
