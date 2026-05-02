using EventoInterface.ViewModels;
using System.Windows;

namespace EventoInterface
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(); // 🔥 ESSENCIAL
        }
    }
}