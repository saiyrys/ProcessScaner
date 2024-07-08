using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using OpenHardwareMonitor.Hardware;
using ProcessScanner.GPUPage;

namespace ProcessScanner.RAMPage
{
    /// <summary>
    /// Логика взаимодействия для RamPage.xaml
    /// </summary>
    public partial class RamPage : Page
    {
        private readonly Computer computer;
        private readonly DispatcherTimer timer;
        public RamPage()
        {
            InitializeComponent();
            computer = new Computer();
            computer.RAMEnabled = true;
            computer.Open();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1); 
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateRAMUsage();
        }

        // Метод отображения круга загрузки на главном экране
        private void UpdateRAMUsage()
        {
            foreach (var hardwareItem in computer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.RAM)
                {
                    hardwareItem.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name == "Memory")
                        {
                            double ramUsage = (double)sensor.Value;
                            UpdateRAMUsageText(ramUsage);
                            UpdateMemoryUsageText(ramUsage);
                            UpdateClipHeight(ramUsage);
                            UpdateEllipseFillColor(ramUsage);
                        }
                    }
                }
            }
        }

        // Вывод о загрузке RAM в процентах
        private void UpdateRAMUsageText(double ramUsage)
        {
            ramUsageText.Text = $"{ramUsage:F0}%";
        }

        // Метод заполнения круга загрузки на главном экране
        private void UpdateMemoryUsageText(double ramUsage)
        {
            double totalMemoryBytes = 0;
            var ramSensor = computer.Hardware.FirstOrDefault(hw => hw.HardwareType == HardwareType.RAM)?.Sensors
                .FirstOrDefault(sensor => sensor.SensorType == SensorType.Load && sensor.Name == "Total Memory");

            if (ramSensor != null && ramSensor.Value.HasValue)
            {
                totalMemoryBytes = ramSensor.Value.Value;

                double usedMemoryBytes = totalMemoryBytes * (ramUsage / 100);

                double freeMemoryGB = (totalMemoryBytes - usedMemoryBytes) / (1024 * 1024 * 1024);

                ramMemoryUsage.Text = $"Память: {freeMemoryGB:F2} GB свободно из {(totalMemoryBytes / (1024 * 1024 * 1024)):F2} GB";
            }
            else
            {
                ramMemoryUsage.Text = "Нет данных о памяти";
            }
        }

        // Заполнение круга
        private void UpdateClipHeight(double ramUsage)
        {
            double clipHeight = (ramUsage / 100) * ellipseRam.Height;
            ellipseRam.Clip = new RectangleGeometry(new Rect(0, ellipseRam.Height - clipHeight, ellipseRam.Width, clipHeight));
        }

        private void UpdateEllipseFillColor(double ramUsage)
        {
            Color fillColor;
            if (ramUsage < 30)
                fillColor = Colors.Green;
            else if (ramUsage < 75)
                fillColor = Colors.Orange;
            else
                fillColor = Colors.Red;

            ellipseRam.Fill = new SolidColorBrush(fillColor);
        }
    }
}
