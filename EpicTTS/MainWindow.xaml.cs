using EpicTTS.ViewModels;

namespace EpicTTS
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(Options options)
        {
            DataContext = new MainViewModel(options);
            InitializeComponent();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            var viewModel = ((MainViewModel)DataContext);
            DataContext = null;
            viewModel.Dispose();
        }
    }
}