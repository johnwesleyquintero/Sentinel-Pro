using System.Windows;
using System.Windows.Controls;
using System.Windows.Media; // Added for VisualTreeHelper

namespace SentinelPro.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            // Find all buttons in the page
            foreach (var button in FindVisualChildren<Button>(this))
            {
                button.Click += (s, e) =>
                {
                    if (button.Content.ToString() == "View Backups")
                    {
                        // Find the Backups radio button in the main window and select it
                        var mainWindow = Application.Current.MainWindow as MainWindow;
                        if (mainWindow != null)
                        {
                            foreach (var radioButton in FindVisualChildren<RadioButton>(mainWindow))
                            {
                                if (radioButton.Content.ToString() == "Backups")
                                {
                                    radioButton.IsChecked = true;
                                    break;
                                }
                            }
                        }
                    }
                };
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}
