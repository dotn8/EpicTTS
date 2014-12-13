using EpicTTS.Models;

namespace EpicTTS
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(Options options)
        {
            DataContext = new MainModel(options);
            InitializeComponent();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            var viewModel = ((MainModel)DataContext);
            DataContext = null;
            viewModel.Dispose();
        }
    }
}