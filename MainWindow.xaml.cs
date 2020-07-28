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
        private bool fullScreen = false;
        private string colorMode = "Gray";

        public MainWindow()
        {
            InitializeComponent();
            grayColorMenuItem.Icon = new Ellipse()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Width = 5,
                Height = 5
            };
        }
        
        private static readonly Regex _regex = new Regex("[^0-9.]+");

        public void DrawPoint(int x, int y, int width, byte shade) {
            Point point = new Point(x, y);
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = width;
            
            ellipse.StrokeThickness = 4;
            Color color = new Color();
            color.A = 255;
            switch (colorMode) {
                case "Red":
                    color.R = shade;
                    color.G = 0;
                    color.B = 0;
                    break;
                case "Green":
                    color.R = 0;
                    color.G = shade;
                    color.B = 0;
                    break;
                case "Blue":
                    color.R = 0;
                    color.G = 0;
                    color.B = shade;
                    break;
                default:
                    color.R = shade;
                    color.G = shade;
                    color.B = shade;
                    break;
            }
            
            Brush brush = new SolidColorBrush(color);
            ellipse.Stroke = brush;
            ellipse.Margin = new Thickness(point.X - (width / 2), point.Y - (width / 2), 0, 0);
            
            canvas.Children.Add(ellipse);
        }

        public void Svaston(int width, byte shade) {
            for (int i = 150; i < 350; i++) {
                DrawPoint(250, i, width, shade);
            }
            for (int i = 150; i < 350; i++) {
                DrawPoint(i, 250, width, shade);
            }
            for (int i = 250; i < 350; i++) {
                DrawPoint(350, i, width, shade);
            }
            for (int i = 150; i < 250; i++) {
                DrawPoint(150, i, width, shade);
            }
            for (int i = 150; i < 250; i++) {
                DrawPoint(i, 350, width, shade);
            }
            for (int i = 250; i < 350; i++) {
                DrawPoint(i, 150, width, shade);
            }
        }
        
        
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void PreviewText(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void Info_OnClick(object sender, RoutedEventArgs e) {
            Svaston(50, 200);
        }
        
        private void FullScreen_OnClick(object sender, RoutedEventArgs e) //TODO change canvas size
        {
            if (fullScreen)
            {
                mainWindow.WindowState = WindowState.Normal;
                fullScreenMenuItem.Icon = null;
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
                fullScreenMenuItem.Icon = new Ellipse()
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Black,
                    Width = 5,
                    Height = 5
                };
            }
            fullScreen = !fullScreen;
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
        
        private void GrayColor_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void RedColor_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void GreenColor_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void BlueColor_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}