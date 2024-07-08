using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Threading;
using ProcessScanner.Services.ScannerService;

namespace ProcessScanner.Services.Scanner
{
    /// <summary>
    /// Логика взаимодействия для SelectSettingScanner.xaml
    /// </summary>
    public partial class SelectSettingScanner : Window
    {
        private ScanResult scanResult;
        private ProcessorScanner processorScanner;
        private GpuScanner gpuScanner;
        private RamScanner ramScanner;
        public SelectSettingScanner()
        {
            InitializeComponent();
            processorScanner = new ProcessorScanner();
            gpuScanner = new GpuScanner();
            ramScanner = new RamScanner();
        }

        // Сканирование выбранной папки
        private async void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;

                var loadingWindow = new LoadingWindow();
                loadingWindow.Show();

                try
                {
                    var scanResults = await Task.Run(() => ScanFolder(selectedPath, loadingWindow.UpdateProgress));
                    ShowScanResults(scanResults);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка сканирования: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    loadingWindow.Close();
                }
            }
        }
        // Реализация сканирования в методе SelectFolder_Click
        private ScanResult ScanFolder(string folderPath, Action<double, string> progressCallback)
        {
            var result = new ScanResult();

            ScanForHiddenItems(folderPath, result, progressCallback);

            return result;
        }
        // Реализация сканирования скрытых фалов и папок в методе SelectFolder_Click
        private void ScanForHiddenItems(string folderPath, ScanResult result, Action<double, string> progressCallback)
        {
            string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            string[] directories = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);
            int totalItems = files.Length + directories.Length;
            int processedItems = 0;
            bool hiddenItemFound = false;

            foreach (var file in files)
            {
                if ((File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    result.AddHiddenFile(file);
                }

                processedItems++;
                double progress = (double)processedItems / totalItems;
                string message = $"Scanning item {processedItems} of {totalItems}: {file}";
                progressCallback(progress, message);
            }

            foreach (var directory in directories)
            {
                if ((File.GetAttributes(directory) & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    result.AddHiddenFolder(directory);
                }

                processedItems++;
                double progress = (double)processedItems / totalItems;
                string message = $"Scanning item {processedItems} of {totalItems}: {directory}";
                progressCallback(progress, message);
            }

            if (result.HiddenFiles.Count == 0 && result.HiddenFolders.Count == 0)
            {
                result.AddMessage("Скрытых файлов или папок не найдено.");
            }
        }

        // Отображение результатов сканирования
        private void ShowScanResults(ScanResult result)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var message in result.Messages)
            {
                sb.AppendLine(message);
            }

            foreach (var hiddenFile in result.HiddenFiles)
            {
                sb.AppendLine("Обнаружен скрытый элемент:");
                sb.AppendLine(hiddenFile);
            }

            foreach (var hiddenFolder in result.HiddenFolders)
            {
                sb.AppendLine("Обнаружена скрытая папка:");
                sb.AppendLine(hiddenFolder);
            }

            System.Windows.MessageBox.Show(sb.ToString(), "Результаты сканирования", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Запуск сканирования с выбранными настройками
        private void StartScanning_Click(object sender, RoutedEventArgs e)
        {
            ScanResults scanResultsWindow = new ScanResults();

            if (cpuCheckBox.IsChecked == true)
            {
                var cpuInfo = ProcessorScanner.ScanCPU();
                scanResultsWindow.UpdateProcessResults(cpuInfo);
            }

            if (gpuCheckBox.IsChecked == true)
            {
                var gpuInfo = GpuScanner.ScanGPU();
                scanResultsWindow.UpdateGPUResults(gpuInfo);
            }
            
            if (ramCheckBox.IsChecked == true)
            {
                var ramInfo = RamScanner.ScanRAM();
                scanResultsWindow.UpdateRamResults(ramInfo);
            }
          
            scanResultsWindow.Show();
        }
    }
}
