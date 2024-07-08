using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScanner.Services.ScannerService
{
    public class ScanResult
    {
        public List<string> HiddenFiles { get; } = new List<string>();
        public List<string> HiddenFolders { get; } = new List<string>(); 
        public List<string> Messages { get; } = new List<string>();

        // Отображение скрытых файлов
        public void AddHiddenFile(string filePath)
        {
            HiddenFiles.Add(filePath);
        }

        // Отображение скрытых папок
        public void AddHiddenFolder(string folderPath) 
        {
            HiddenFolders.Add(folderPath);
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

    }
}
