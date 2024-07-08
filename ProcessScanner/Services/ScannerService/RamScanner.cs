using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScanner.Services.ScannerService
{
    public class RamScanner
    {
        public RamScanner()
        {

        }
        // Функция сканирования оперативной памяти
        public static List<RAMInfo> ScanRAM()
        {
            List<RAMInfo> ramInfos = new List<RAMInfo>();

            // Запрос на получение данных из оперативной памяти
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process WHERE Name <> '_Total'");

            foreach (ManagementObject obj in processSearcher.Get())
            {
                string processName = obj["Name"] as string;
                long workingSetPrivate = Convert.ToInt64(obj["WorkingSetPrivate"]);

                double ramUsageMB = workingSetPrivate / (1024.0 * 1024.0);

                ramInfos.Add(new RAMInfo { Name = processName, RamUsage = ramUsageMB });
            }

            ramInfos.Sort((x, y) => y.RamUsage.CompareTo(x.RamUsage));
            ramInfos = ramInfos.Take(16).ToList();
            return ramInfos;
        }
        // Функция генерации HTML отчёта вызываемая из главного окна
        public static void GenerateHtmlReport(List<RAMInfo> ramInfos, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<html><head><title>Ram Usage Report</title></head><body>");
                writer.WriteLine("<h1>Ram Usage Report</h1>");
                writer.WriteLine("<table border='1'><tr><th>Process Name</th><th>Ram Usage (MB)</th></tr>");

                foreach (var info in ramInfos)
                {
                    writer.WriteLine($"<tr><td>{info.Name}</td><td>{info.RamUsage}%</td></tr>");
                }

                writer.WriteLine("</table></body></html>");
            }
        }
    }
}
