using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private string colorMode = "Gray";
        private Figure figure = new Figure();
        private int stepX = 4;
        private int stepY = 4;
        private Tomograph tm = new Tomograph();
        private static readonly Regex posNums = new Regex("[^0-9,]+");
        private static readonly Regex allNums = new Regex("[^0-9,-]+");
        private List<List<double>> Values = new List<List<double>>();
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
        private static bool IsTextAllowed(string text, int sign)
        {
            if (sign == 1)
            {
                return !posNums.IsMatch(text);
            }
            else
            {
                return !allNums.IsMatch(text);
            }
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
                    try
                    {
                        Values = JsonConvert.DeserializeObject<List<List<double>>>(json);
                    }
                    catch (JsonException _e)
                    {
                        MessageBox.Show("Неверный формат файла", "Ошибка", 
                                                MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
            tm.CalculateRadiationDensity();
            string output = JsonConvert.SerializeObject(tm.H);
            byte[] fileout = new byte[output.Length];
            int k = 0;
            foreach (var letter in output)
            {
                fileout[k] = (byte)letter;
                k++;
            }
            var file = File.Create("2.json");
            file.Write(fileout, 0, output.Length);
            
            Indicator id = new Indicator(tm);
            id.CalculateHeterogenityIndicator();
            Values = id.IndicatorValues;
            stepX = 400 / id.Nx;
            stepY = 400 / id.Ny;
            double minValue = double.MaxValue;
            double maxValue = double.MinValue;
            for (int i = 0; i < Values.Count; i++)
            {
                for (int j = 0; j < Values[i].Count; j++)
                {
                    if (Values[i][j] < minValue)
                    {
                        minValue = Values[i][j];
                    }
                    if (Values[i][j] > maxValue)
                    {
                        maxValue = Values[i][j];
                    }
                }
            }

            for (int i = 0; i < Values.Count; i++)
            {
                for (int j = 0; j < Values[i].Count; j++)
                {
                    double val = Values[i][j] - minValue;
                    byte shade = (byte) (255 * val / (maxValue - minValue));
                    DrawPoint(i * stepX - 200 - 2, j * stepY + ((400 - Values[i].Count * stepY) / 2) - 200, 4, shade);
                }
            }
        }

        private void FigureConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            double x = 0.0, y = 0.0, a = 0.0, b = 0.0;
            if (txtBox1.Text != "")
            {
                x = double.Parse(txtBox1.Text);
                if (x < -1 || x > 1)
                {
                    MessageBox.Show("Введите значения от -1 до 1", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            if (txtBox2.Text != "")
            {
                y = double.Parse(txtBox2.Text);
                if (y < -1 || y > 1)
                {
                    MessageBox.Show("Введите значения от -1 до 1", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            if (txtBox3.Text != "")
            {
                a = double.Parse(txtBox3.Text);
                if (a < -1 || a > 1)
                {
                    MessageBox.Show("Введите значения от -1 до 1", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            if (txtBox4.Text != "")
            {
                b = double.Parse(txtBox4.Text);
                if (b < -1 || b > 1)
                {
                    MessageBox.Show("Введите значения от -1 до 1", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            this.figure.x = x;
            this.figure.y = -y;
            this.figure.a = a;
            this.figure.b = b;
            tm.figure = this.figure;
        }

        private void AddTextBoxes(string fig)
        {
            switch (fig)
            {
                case "rec":
                    txtBlock1.Visibility = Visibility.Visible;
                    txtBlock2.Visibility = Visibility.Visible;
                    txtBlock3.Text = "Длина: ";
                    txtBlock3.Visibility = Visibility.Visible;
                    txtBlock4.Text = "Ширина: ";
                    txtBlock4.Visibility = Visibility.Visible;
                    txtBox1.Visibility = Visibility.Visible;
                    txtBox2.Visibility = Visibility.Visible;
                    txtBox3.Visibility = Visibility.Visible;
                    txtBox4.Visibility = Visibility.Visible;
                    figureConfirmButton.Visibility = Visibility.Visible;
                    break;
                case "ell":
                    txtBlock1.Visibility = Visibility.Visible;
                    txtBlock2.Visibility = Visibility.Visible;
                    txtBlock3.Text = "Большая п-ось: ";
                    txtBlock3.Visibility = Visibility.Visible;
                    txtBlock4.Text = "Малая п-ось: ";
                    txtBlock4.Visibility = Visibility.Visible;
                    txtBox1.Visibility = Visibility.Visible;
                    txtBox2.Visibility = Visibility.Visible;
                    txtBox3.Visibility = Visibility.Visible;
                    txtBox4.Visibility = Visibility.Visible;
                    figureConfirmButton.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void FigureOption_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = "";
            List<double> coords = new List<double>();
            switch (figureOption.SelectedIndex)
            {
                case 1:
                    figure.name = "Rectangle";
                    AddTextBoxes("rec");
                    break;
                case 2:
                    figure.name = "Ellipse";
                    AddTextBoxes("ell");
                    break;
                default:
                    break;
            }
        }

        private void PosNum_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, 1);
        }
        
        private void AllNum_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, -1);
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
        }
    }
}