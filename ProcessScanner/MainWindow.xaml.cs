using ProcessScanner.CPUPage;
using ProcessScanner.GPUPage;
using ProcessScanner.RAMPage;
using ProcessScanner.Services.Scanner;
using ProcessScanner.Services.ScannerService;
using System;
using System.IO;
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
using ProcessScanner.Services;
using System.Diagnostics;

namespace ProcessScanner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.NavigationService.Navigate(new CpuPage());

        }

        private void HTMLReport(object sender, RoutedEventArgs e)
        {
            List<GPUInfo> gpuInfos = GpuScanner.ScanGPU();
            List<ProcessInfo> cpuInfos = ProcessorScanner.ScanCPU();
            List<RAMInfo> ramInfos = RamScanner.ScanRAM();

            // Определение последнего выполненного сканирования
            if (gpuInfos.Any())
            {
                string filePathgpu = "gpu_report.html"; // Путь к файлу HTML отчета
                GpuScanner.GenerateHtmlReport(gpuInfos, filePathgpu);
                string destinationPathGPU = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(filePathgpu));
                File.Copy(filePathgpu, destinationPathGPU, true);
                MessageBox.Show("Отчет о GPU успешно создан и сохранен на рабочем столе.", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (cpuInfos.Any())
            {
                string filePathcpu = "cpu_report.html"; // Путь к файлу HTML отчета
                ProcessorScanner.GenerateHtmlReport(cpuInfos, filePathcpu);
                string destinationPathCPU = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(filePathcpu));
                File.Copy(filePathcpu, destinationPathCPU, true);
                MessageBox.Show("Отчет о CPU успешно создан и сохранен на рабочем столе.", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (ramInfos.Any())
            {
                string filePathram = "ram_report.html"; // Путь к файлу HTML отчета
                RamScanner.GenerateHtmlReport(ramInfos, filePathram);
                string destinationPathRAM = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(filePathram));
                File.Copy(filePathram, destinationPathRAM, true);
                MessageBox.Show("Отчет о RAM успешно создан и сохранен на рабочем столе.", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CPU(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content is CpuPage)
            {
                MessageBox.Show("Вы уже на странице c информацией о \nПроцессоре.");
            }
            else
            {
                mainFrame.Navigate(new CpuPage());
            }
        }

        private void GPU(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content is GpuPage)
            {
                MessageBox.Show("Вы уже на странице c информацией о \nВидеокарте.");
            }
            else
            {
                mainFrame.Navigate(new GpuPage());
            }
        }

        private void RAM(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content is RamPage)
            {
                MessageBox.Show("Вы уже на странице c информацией о \nОперативной памяти.");
            }
            else
            {
                mainFrame.Navigate(new RamPage());
            }
        }

        private void Start_Scann(object sender, RoutedEventArgs e)
        {
            var scanner = new SelectSettingScanner();
            if (scanner.IsActive)
            {
                MessageBox.Show("Окно уже запущено");
            }
            else
            {
                scanner.Show();
            }
        }

        private void user_Manual(object sender, RoutedEventArgs e)
        {
            string fileName = "Руководство.docx";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(filePath);
                }
                else
                {
                    MessageBox.Show("Файл не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
