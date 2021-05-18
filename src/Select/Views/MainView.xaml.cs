using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using Select.Services.Input;
using Select.ViewModels;

namespace Select.Views
{
    public partial class MainView : Window
    {
        private readonly MainViewModel _mainViewViewModel;
        
        public MainView()
        {
            InitializeComponent();
            _mainViewViewModel = new MainViewModel();
            DataContext = _mainViewViewModel;
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _mainViewViewModel?.Initialize();
        }
    }
}