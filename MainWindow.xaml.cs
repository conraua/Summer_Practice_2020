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
        private bool meshDrawn = false;
        private string colorMode = "Gray";
        private double rayPower = 1;
        private int Nx = 100;
        private int Ny = 100;
        private int stepX;
        private int stepY;
        private static readonly Regex _regex = new Regex("[^0-9.]+");
        private List<List<double>> H = new List<List<double>>();
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
            stepX = 400 / Nx;
            stepY = 400 / Ny;
        }

        public void DrawPoint(int x, int y, int width, byte shade) 
        {
            if (x*x + y*y <= 40000)
            {
                Point point = new Point(x, y);
                Ellipse ellipse = new Ellipse();
                ellipse.Width = width;
                ellipse.Height = width;

                ellipse.StrokeThickness = 1;
                Color color = new Color();
                color.A = 255;
                switch (colorMode)
                {
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
                        color.R = (byte) (255 - shade);
                        color.G = (byte) (255 - shade);
                        color.B = (byte) (255 - shade);
                        break;
                }

                Brush brush = new SolidColorBrush(color);
                ellipse.Stroke = brush;
                ellipse.Fill = brush;
                ellipse.Margin = new Thickness(250 + point.X, 250 + point.Y, 0, 0);

                canvas.Children.Add(ellipse);
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

        private void ClearCanvas()
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
        private void Info_OnClick(object sender, RoutedEventArgs e)
        {
            if (!infoShown)
            {
                
            }
            else
            {
                
            }
            infoShown = !infoShown;
        }

        private void DrawMesh_OnClick(object sender, RoutedEventArgs e)
        {
            if (!meshDrawn)
            {
                drawMeshMenuItem.Icon = new Ellipse()
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Black,
                    Width = 5,
                    Height = 5
                };
                for (int i = 0; i < 400; i += stepX)
                {
                    for (int j = 0; j < 400; j += stepY)
                    {
                        DrawPoint(i - 200, j - 200, 2, 255);
                    }
                }
            }
            else
            {
                drawMeshMenuItem.Icon = null;
                ClearCanvas();
            }

            meshDrawn = !meshDrawn;
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

        private void OpenFile_OnClick(object sender, RoutedEventArgs e) 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                
                string path = openFileDialog.FileName;
                if (File.Exists(path)) {
                    string json = File.ReadAllText(path);
                    H = JsonConvert.DeserializeObject<List<List<double>>>(json);
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