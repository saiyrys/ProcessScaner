using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using OpenHardwareMonitor.Hardware;

namespace ProcessScanner.CPUPage
{
    /// <summary>
    /// Логика взаимодействия для CpuPage.xaml
    /// </summary>
    public partial class CpuPage : Page
    {
        private readonly Computer computer;
        private readonly DispatcherTimer timer;

        public CpuPage()
        {
            InitializeComponent();

            computer = new Computer();
            computer.CPUEnabled = true;
            computer.Open();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCpuUsage();

        }

        // Метод отображения круга загрузки на главном экране
        private void UpdateCpuUsage()
        {
            foreach (var hardwareItem in computer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.CPU)
                {
                    hardwareItem.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                        {
                            float cpuUsage = (float)sensor.Value;
                            UpdateCpuUsageText(cpuUsage);
                            UpdateClipHeight(cpuUsage);
                            UpdateEllipseFillColor(cpuUsage);
                        }
                    }
                }
            }
        }

        // Вывод о загрузке CPU в процентах
        private void UpdateCpuUsageText(float cpuUsage)
        {
            cpuUsages.Text = $"{cpuUsage:F0}%";
        }

        // Метод заполнения круга загрузки на главном экране
        private void UpdateClipHeight(float usage)
        {
            double clipHeight = (usage / 100) * ellipseСPU.Height;
            ellipseСPU.Clip = new RectangleGeometry(new Rect(0, ellipseСPU.Height - clipHeight, ellipseСPU.Width, clipHeight));
        }

        private void UpdateEllipseFillColor(float usage)
        {
            Color fillColor;
            if (usage < 30)
                fillColor = Colors.Green;
            else if (usage < 75)
                fillColor = Colors.Orange;
            else
                fillColor = Colors.Red;

            ellipseСPU.Fill = new SolidColorBrush(fillColor);
        }
    }
}
