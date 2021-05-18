using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using Select.Services.Input;
using Select.ViewModels;

namespace Select.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel)
            {
                mainViewModel?.Initialize();
            }
        }
    }
}