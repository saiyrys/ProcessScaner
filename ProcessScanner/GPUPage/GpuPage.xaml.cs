using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using OpenHardwareMonitor.Hardware;

namespace ProcessScanner.GPUPage
{
    /// <summary>
    /// Логика взаимодействия для GPUWindow.xaml
    /// </summary>
    public partial class GpuPage : Page
    {
        private readonly Computer computer;
        private readonly DispatcherTimer timer;

        public GpuPage()
        {
            InitializeComponent();

            computer = new Computer();
            computer.GPUEnabled = true;
            computer.Open();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateGPUUsage();
        }

        // Метод отображения круга загрузки на главном экране
        private void UpdateGPUUsage()
        {
            foreach (var hardwareItem in computer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.GpuNvidia || hardwareItem.HardwareType == HardwareType.GpuAti)
                {
                    hardwareItem.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                        {
                            double gpuUsage = (double)sensor.Value;
                            UpdateGPUUsageText(gpuUsage);
                            UpdateClipHeight(gpuUsage);
                            UpdateEllipseFillColor(gpuUsage);
                        }
                    }
                }
            }
        }
        // Вывод о загрузке GPU в процентах
        private void UpdateGPUUsageText(double gpuUsages)
        {
            gpuUsage.Text = $"{gpuUsages:F0}%";
        }

        // Метод заполнения круга загрузки на главном экране
        private void UpdateClipHeight(double usage)
        {
            double clipHeight = (usage / 100) * ellipseGPU.Height;
            ellipseGPU.Clip = new RectangleGeometry(new Rect(0, ellipseGPU.Height - clipHeight, ellipseGPU.Width, clipHeight));
        }

        private void UpdateEllipseFillColor(double usage)
        {
            Color fillColor;
            if (usage < 30)
                fillColor = Colors.Green;
            else if (usage < 75)
                fillColor = Colors.Orange;
            else
                fillColor = Colors.Red;

            ellipseGPU.Fill = new SolidColorBrush(fillColor);
        }

    }
}
