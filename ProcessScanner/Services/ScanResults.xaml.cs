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
using System.Windows.Shapes;

namespace ProcessScanner.Services
{
    /// <summary>
    /// Логика взаимодействия для ScanResults.xaml
    /// </summary>
    public partial class ScanResults : Window
    {
        public ScanResults()
        {
            InitializeComponent();
        }

        // Методы отображения  результатов в DATAGRID
        public void UpdateProcessResults(List<ProcessInfo> processInfos)
        {
            cpuScanResults.ItemsSource = processInfos;
        }
        public void UpdateGPUResults(List<GPUInfo> GpuInfos)
        {
            gpuScanResults.ItemsSource = GpuInfos;
        }
        public void UpdateRamResults(List<RAMInfo> RamInfos)
        {
            ramScanResults.ItemsSource = RamInfos;
        }
    }
    public class ProcessInfo
    {
        public string Name { get; set; }
        public float CpuUsage { get; set; }
    }

    public class GPUInfo
    {
        public string Name { get; set; }
        public float GpuUsage { get; set; }
    }
    public class RAMInfo
    {
        public string Name { get; set; }
        public double RamUsage { get; set; }
    }
}
