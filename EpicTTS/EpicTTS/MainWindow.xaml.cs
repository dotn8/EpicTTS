using EpicTTS.Models;

namespace EpicTTS
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = new MainModel();
            InitializeComponent();
        }
    }
}