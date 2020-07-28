using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SummerPractice2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool darkTheme = false;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private static readonly Regex _regex = new Regex("[^0-9.]+");
        
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void PreviewText(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void Info_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void View_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void DarkTheme_OnClick(object sender, RoutedEventArgs e)
        {
            if (darkTheme)
            {
                mainWindow.Background = Brushes.White;
                darkThemeMenuItem.Icon = null;
            }
            else
            {
                mainWindow.Background = Brushes.DarkGray;
                darkThemeMenuItem.Icon = new Ellipse()
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Black,
                    Width = 5,
                    Height = 5
                };
            }
            darkTheme = !darkTheme;
        }
    }
}