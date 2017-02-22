using System;
using System.Windows;
using EpicTTS.ViewModels;

namespace EpicTTS
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), options))
            {
                Console.Error.Write(options.GetUsage());
                MessageBox.Show(options.GetUsage());
                Environment.Exit(1);
            }

            if (options.Headless)
            {
                var model = new MainViewModel(options);
                model.Speak();
                Environment.Exit(0);
            }
            else
            {
                var mainWindow = new Views.MainWindow(options);
                mainWindow.Show();
            }

            base.OnStartup(e);
        }
    }
}