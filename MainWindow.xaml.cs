using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace AsyncAwaitDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            //We need to do some fancy calculation the slows down the UI.
            StatusLabel.Content = "START - Running Syncronous";
            Thread.Sleep(5000);
            StatusLabel.Content = "END - Running Syncronous";

        }


        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = "START - Running Asyncronous";
            await Task.Run(() => Thread.Sleep(5000));
            StatusLabel.Content = "END - Running Asyncronous";
        }

    }
}
