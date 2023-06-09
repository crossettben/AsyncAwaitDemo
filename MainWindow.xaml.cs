﻿using AsyncAwaitDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
        private List<string> websites { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            this.websites = PrepData();
        }

        private List<string> PrepData()
        {
            List<string> output = new List<string>();
            output.Add("https://www.google.com");
            output.Add("https://www.yahoo.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.stackoverflow.com");
            output.Add("https://www.codeproject.com");
            return output;
        }
        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTextArea();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            CurrentStatusLabel.Content = "START - Running Syncronous";
            foreach (var website in websites)
            {
                //TODO: Find a way to download website syncronously
                WebsiteDataModel model = DownloadWebsite(website);
                UpdateTextArea(model);
            }
            CurrentStatusLabel.Content = "END - Running Syncronous";

            watch.Stop();
            var elaspsedMs = watch.ElapsedMilliseconds;
            ElapsedTime.Content = $"{elaspsedMs} ms";

        }
        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTextArea();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            CurrentStatusLabel.Content = "START - Running Asyncronous";

            foreach (var website in websites)
            {
                WebsiteDataModel model = await DownloadWebsiteAsync(website);
                UpdateTextArea(model); 
            }

            CurrentStatusLabel.Content = "END - Running Asyncronous";

            watch.Stop();
            var elaspsedMs = watch.ElapsedMilliseconds;
            ElapsedTime.Content = $"{elaspsedMs} ms";

        }
        private async void AsyncButtonInParallel_Click(object sender, RoutedEventArgs e)
        {
            ClearTextArea();

            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            CurrentStatusLabel.Content = "START - Running Asyncronous (In Parallel)";

            foreach (var website in websites)
            {
                tasks.Add(Task.Run(() => DownloadWebsiteAsync(website)));
            }

            var results = await Task.WhenAll(tasks); //Wait for all tasks to complete

            foreach (var model in results)
            {
                UpdateTextArea(model);
            }

            CurrentStatusLabel.Content = "END - Running Asyncronous (In Parallel)";
            
            watch.Stop();
            
            var elaspsedMs = watch.ElapsedMilliseconds;
            ElapsedTime.Content = $"{elaspsedMs} ms";

        }
        private void ClearTextArea()
        {
            tbMultiLine.Text = String.Empty;
        }
        private void UpdateTextArea(WebsiteDataModel model)
        {
            tbMultiLine.Text += $"{model.Url} - Downloaded {model.Data.Length} characters. {Environment.NewLine}";
        }
        private WebsiteDataModel DownloadWebsite(string websiteUrl)
        {
            WebClient client = new WebClient();
            string pageData = client.DownloadString(websiteUrl);

            return new WebsiteDataModel()
            {
                Url = websiteUrl,
                Data = pageData
            };
        }
        private async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteUrl)
        {
            WebClient client = new WebClient();
            string pageData = await Task.Run(() => client.DownloadString(websiteUrl)); //Turning a non-async into async.

            return new WebsiteDataModel()
            {
                Url = websiteUrl,
                Data = pageData
            };
        }

    }
}
