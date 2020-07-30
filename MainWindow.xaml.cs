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
using System.Windows.Media.Imaging;
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
        private string colorMode = "Gray";
        private Figure figure = new Figure();
        private int stepX = 4;
        private int stepY = 4;
        private int Nx = 0;
        private int Ny = 0;
        private Tomograph tm = new Tomograph();
        private static readonly Regex posNums = new Regex("[^0-9,]+");
        private static readonly Regex allNums = new Regex("[^0-9,-]+");
        private List<List<double>> indValues = new List<List<double>>();
        private List<List<double>> tomValues = new List<List<double>>();
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
        
        public void ExportToPng(Uri path, Canvas surface)
        {
            if (path == null) return;

            Transform transform = surface.LayoutTransform;
            surface.LayoutTransform = null;

            Size size = new Size(surface.Width, surface.Height);
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            RenderTargetBitmap renderBitmap = 
                new RenderTargetBitmap(
                    (int)size.Width, 
                    (int)size.Height, 
                    96d, 
                    96d, 
                    PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            using (FileStream outStream = new FileStream(path.LocalPath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }

            surface.LayoutTransform = transform;
            surface.Margin = new Thickness(455, 10, 0, 0);
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
            Window window = new Window();
            Grid grid = new Grid();
            window.Content = grid;
            TextBlock txtBlock = new TextBlock();
            txtBlock.Text = "Меня забыли изменить";
            grid.Children.Add(txtBlock);
            window.Show();
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
                        tomValues = JsonConvert.DeserializeObject<List<List<double>>>(json);
                        indFigButton.Visibility = Visibility.Hidden;
                        indFileButton.Visibility = Visibility.Visible;
                    }
                    catch (JsonException _e)
                    {
                        MessageBox.Show("Неверный формат файла", "Ошибка", 
                                                MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        
        private void SaveImage_OnClick(object sender, RoutedEventArgs e) 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                ExportToPng(new Uri(saveFileDialog.FileName, UriKind.Absolute), canvas);
            }
        }

        private void SaveJson_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                
                string output = JsonConvert.SerializeObject(tomValues);
                byte[] fileout = new byte[output.Length];
                int k = 0;
                foreach (var letter in output)
                {
                    fileout[k] = (byte) letter;
                    k++;
                }
                var file = File.Create(saveFileDialog.FileName);
                file.Write(fileout, 0, output.Length);
                file.Close();
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

        private void TomButton_OnClick(object sender, RoutedEventArgs e)
        {
            tm.H.Clear();
            tm.figure = figure;
            tm.CalculateRadiationDensity();
            tomValues = tm.H;
            indFigButton.Visibility = Visibility.Visible;
            indFileButton.Visibility = Visibility.Hidden;
        }

        private void IndFigButton_OnClick(object sender, RoutedEventArgs e)
        {
            Indicator id = new Indicator(tm);
            id.CalculateHeterogenityIndicator();
            indValues = id.IndicatorValues;
            Nx = id.Nx;
            Ny = id.Ny;
            drawButton.Visibility = Visibility.Visible;
        }
        private void IndFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            Indicator id = new Indicator(tomValues);
            id.CalculateHeterogenityIndicator();
            indValues = id.IndicatorValues;
            Nx = id.Nx;
            Ny = id.Ny;
            drawButton.Visibility = Visibility.Visible;
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
                if (figure.name == "Rectangle")
                {
                    if (a < -2 || a > 2)
                    {
                        MessageBox.Show("Введите значения от -2 до 2", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    if (a < -1 || a > 1)
                    {
                        MessageBox.Show("Введите значения от -1 до 1", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                }
            }
            if (txtBox4.Text != "")
            {
                b = double.Parse(txtBox4.Text);
                if (b < -2 || b > 2)
                {
                    MessageBox.Show("Введите значения от -2 до 2", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (b < -1 || b > 1)
                    {
                        MessageBox.Show("Введите значения от -1 до 1", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                }
            }

            tomButton.Visibility = Visibility.Visible;
            this.figure.x = x;
            this.figure.y = -y;
            this.figure.a = a;
            this.figure.b = b;
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
            clearButton.Visibility = Visibility.Hidden;
        }
        
        private void DrawButton_OnClick(object sender, RoutedEventArgs e)
        {
            stepX = 400 / Nx;
            stepY = 400 / Ny;
            double minValue = double.MaxValue;
            double maxValue = double.MinValue;
            for (int i = 0; i < indValues.Count; i++)
            {
                for (int j = 0; j < indValues[i].Count; j++)
                {
                    if (indValues[i][j] < minValue)
                    {
                        minValue = indValues[i][j];
                    }
                    if (indValues[i][j] > maxValue)
                    {
                        maxValue = indValues[i][j];
                    }
                }
            }

            for (int i = 0; i < indValues.Count; i++)
            {
                for (int j = 0; j < indValues[i].Count; j++)
                {
                    double val = indValues[i][j] - minValue;
                    byte shade = (byte) (255 * val / (maxValue - minValue));
                    DrawPoint(i * stepX - 200 - 2, 
                        j * stepY + ((400 - indValues[i].Count * stepY) / 2) - 200, 4, shade);
                }
            }

            clearButton.Visibility = Visibility.Visible;
            drawButton.Visibility = Visibility.Hidden;
        }
    }
}