using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvAPIWrapper;
using NvAPIWrapper.GPU;
using System.Windows;
using System.Windows.Threading;
using System.Management;
using System.IO;

namespace ProcessScanner.Services
{
    public class GpuScanner
    {
       
        public GpuScanner()
        {
           
        }
        // Функция сканирования видеокарты на процессы, а так же скрытые выполняемые процессы
        public static List<GPUInfo> ScanGPU()
        {
            List<GPUInfo> gpuInfos = new List<GPUInfo>();

            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process WHERE Name <> '_Total'");
            float totalProcessorTime = 0.0f;

            foreach (ManagementObject obj in processSearcher.Get())
            {
                totalProcessorTime += Convert.ToSingle(obj["PercentProcessorTime"]);
            }

            foreach (ManagementObject obj in processSearcher.Get())
            {
                string processName = obj["Name"] as string;
                float processProcessorTime = Convert.ToSingle(obj["PercentProcessorTime"]);
                float percentUsage = (processProcessorTime / totalProcessorTime) * 100.0f;

                gpuInfos.Add(new GPUInfo { Name = processName, GpuUsage = percentUsage });
            }

            gpuInfos.Sort((x, y) => y.GpuUsage.CompareTo(x.GpuUsage));
            gpuInfos = gpuInfos.Take(16).ToList();
            return gpuInfos;
        }
        // Функция генерации HTML отчёта вызываемая из главного окна
        public static void GenerateHtmlReport(List<GPUInfo> gpuInfos, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<html><head><title>GPU Usage Report</title></head><body>");
                writer.WriteLine("<h1>GPU Usage Report</h1>");
                writer.WriteLine("<table border='1'><tr><th>Process Name</th><th>GPU Usage (%)</th></tr>");

                foreach (var info in gpuInfos)
                {
                    writer.WriteLine($"<tr><td>{info.Name}</td><td>{info.GpuUsage}%</td></tr>");
                }

                writer.WriteLine("</table></body></html>");
            }
        }
    }
}



