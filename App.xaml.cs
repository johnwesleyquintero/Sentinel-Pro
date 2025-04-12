using System.Windows;

namespace SentinelPro
{
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
