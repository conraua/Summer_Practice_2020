using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using static SummerPractice2020.Value;
using static SummerPractice2020.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SummerPractice2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool darkTheme = false;
        private bool fullScreen = false;
        private bool infoShown = false;
        private string colorMode = "Gray";
        private float rayPower = 1;
        private Values values = new Values();
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
            
            ellipse.StrokeThickness = 1;
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
                    color.R = (byte)(255 - shade);
                    color.G = (byte)(255 - shade);
                    color.B = (byte)(255 - shade);
                    break;
            }
            
            Brush brush = new SolidColorBrush(color);
            ellipse.Stroke = brush;
            ellipse.Fill = brush;
            ellipse.Margin = new Thickness(point.X - (width / 2.0), point.Y - (width / 2.0), 0, 0);
            
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

        private void Info_OnClick(object sender, RoutedEventArgs e)
        {
            if (!infoShown)
            {
                Svaston(50, 255);
            }
            else
            {
                canvas.Children.Clear();
                Ellipse ellipse = new Ellipse();
                ellipse.Width = 400;
                ellipse.Height = 400;
                ellipse.Stroke = Brushes.Black;
                ellipse.Fill = Brushes.Snow;
                ellipse.Margin = new Thickness(50);
                canvas.Children.Add(ellipse);
            }
            infoShown = !infoShown;
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

        private void OpenFile_OnClick(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                
                string path = openFileDialog.FileName;
                if (File.Exists(path) == true) {
                    string json = File.ReadAllText(path);
                    var structure = JsonConvert.DeserializeObject<List<Tuple<Tuple<int, int>, List<Value>>>>(json);
                    

                }
            }
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
            grayColorMenuItem.Icon = new Ellipse()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Width = 5,
                Height = 5
            };
            redColorMenuItem.Icon = null;
            greenColorMenuItem.Icon = null;
            blueColorMenuItem.Icon = null;
            colorMode = "Gray";
        }
        
        private void RedColor_OnClick(object sender, RoutedEventArgs e)
        {
            grayColorMenuItem.Icon = null;
            redColorMenuItem.Icon = new Ellipse()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Width = 5,
                Height = 5
            };
            greenColorMenuItem.Icon = null;
            blueColorMenuItem.Icon = null;
            colorMode = "Red";
        }
        
        private void GreenColor_OnClick(object sender, RoutedEventArgs e)
        {
            grayColorMenuItem.Icon = null;
            redColorMenuItem.Icon = null;
            greenColorMenuItem.Icon = new Ellipse()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Width = 5,
                Height = 5
            };
            blueColorMenuItem.Icon = null;
            colorMode = "Green";
        }
        
        private void BlueColor_OnClick(object sender, RoutedEventArgs e)
        {
            grayColorMenuItem.Icon = null;
            redColorMenuItem.Icon = null;
            greenColorMenuItem.Icon = null;
            blueColorMenuItem.Icon = new Ellipse()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Width = 5,
                Height = 5
            };
            colorMode = "Blue";
        }

        private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            rayPower = float.Parse(rayPowerTextBox.Text);
            rayPowerTextBox.Text = "";
        }
    }
}