using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProcessScanner.Services
{
    public class ProcessorScanner
    {
        
        
        public event EventHandler<List<ProcessInfo>> ProcessInfoUpdated;
        public ProcessorScanner()
        {

        }

        private const double SuspiciousCPULoadThreshold = 50.0;

        // Функция сканирования процессора на процессы, а так же скрытые выполняемые процессы
        public static List<ProcessInfo> ScanCPU()
        {
            List<ProcessInfo> processInfos = new List<ProcessInfo>();

            Process[] processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                try
                {
                    float cpuUsage = 0.0f;

                    if (process.TotalProcessorTime.TotalMilliseconds > 0)
                    {
                        cpuUsage = (float)process.TotalProcessorTime.TotalMilliseconds / (float)DateTime.Now.Subtract(process.StartTime).TotalMilliseconds * 100.0f;
                    }
                    if (cpuUsage >= SuspiciousCPULoadThreshold)
                    {
                        processInfos.Add(new ProcessInfo { Name = process.ProcessName, CpuUsage = cpuUsage });
                    }
                    else
                    {
                        processInfos.Add(new ProcessInfo { Name = process.ProcessName, CpuUsage = cpuUsage });
                    }
                }
                catch (Exception ex)
                {
                    processInfos.Add(new ProcessInfo { Name = $"Ошибка: {ex.Message}", CpuUsage = 0 });
                }
            }
            processInfos.Sort((x, y) => y.CpuUsage.CompareTo(x.CpuUsage));
            processInfos = processInfos.Take(16).ToList();
            return processInfos;
        }
        // Функция генерации HTML отчёта вызываемая из главного окна
        public static void GenerateHtmlReport(List<ProcessInfo> cpuInfos, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<html><head><title>CPU Usage Report</title></head><body>");
                writer.WriteLine("<h1>CPU Usage Report</h1>");
                writer.WriteLine("<table border='1'><tr><th>Process Name</th><th>Cpu Usage (%)</th></tr>");

                foreach (var info in cpuInfos)
                {
                    writer.WriteLine($"<tr><td>{info.Name}</td><td>{info.CpuUsage}%</td></tr>");
                }

                writer.WriteLine("</table></body></html>");
            }
        }


    }
}



