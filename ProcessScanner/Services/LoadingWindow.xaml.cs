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
    /// Логика взаимодействия для LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        // Реализация отображения процесса сканирования
        public void UpdateProgress(double progress, string message)
        {
            Dispatcher.Invoke(() =>
            {
                progressBar.Value = progress * 100;
                loadingText.Text = message;
            });
        }
    }
}
