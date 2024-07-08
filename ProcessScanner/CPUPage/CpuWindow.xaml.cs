using ProcessScanner.GPUPage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProcessScanner.CPUPage
{
    /// <summary>
    /// Логика взаимодействия для CpuWindow.xaml
    /// </summary>
    public partial class CpuWindow : Window
    {
        private readonly PerformanceCounter cpuCounter;
        private readonly DispatcherTimer timer;
        
        public CpuWindow()
        {
            InitializeComponent();
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            // Устанавливаем формат для более точных данных

            // Инициализируем таймер для обновления данных и интерфейса
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // обновляем каждую секунду
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Получаем текущее значение загрузки процессора
            float cpuUsage = cpuCounter.NextValue();

            // Обновляем текст для отображения загрузки процессора
            cpuUsageText.Text = $"{cpuUsage:F0}%";

            // Вычисляем высоту обрезающего прямоугольника в зависимости от загрузки процессора
            double clipHeight = (cpuUsage / 100) * ellipse.Height;
            ellipse.Clip = new RectangleGeometry(new Rect(0, ellipse.Height - clipHeight, ellipse.Width, clipHeight));

            // Изменяем цвет заливки круга в зависимости от загрузки процессора
            Color fillColor;
            if (cpuUsage < 30)
                fillColor = Colors.Green;
            else if (cpuUsage < 75)
                fillColor = Colors.Orange;
            else
                fillColor = Colors.Red;

            ellipse.Fill = new SolidColorBrush(fillColor);

        }

        private void start_Scann(object sender, RoutedEventArgs e)
        {

        }

        private void HTMLReport(object sender, RoutedEventArgs e)
        {

        }

        private void TXTReport(object sender, RoutedEventArgs e)
        {

        }

        private void WordReport(object sender, RoutedEventArgs e)
        {

        }

        private void CPU(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content is CpuPage)
            {
                MessageBox.Show("Вы уже на странице CPU.");
            }
            else
            {
                // Переходим на страницу CPU и очищаем остальные элементы
                mainFrame.Navigate(new CpuPage());
                CircleGrid.Children.Clear();
            }
        }

        private void GPU(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GPUWindow());
            CircleGrid.Children.Clear();
        }

        private void RAM(object sender, RoutedEventArgs e)
        {

        }

        private void Network(object sender, RoutedEventArgs e)
        {

        }
    }
}
