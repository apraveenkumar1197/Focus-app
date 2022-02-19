using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Focus_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Timer timer;
        String milliSeconds;

        public MainWindow()
        {
            InitializeComponent();
            init();
            String fileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/focus.apk";
            using (StreamReader sr = File.OpenText(fileName))
            {
                milliSeconds = sr.ReadToEnd();

            }
            timeToAlarm.Text = milliSeconds.Remove(milliSeconds.Length - 3, 3);
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String fileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/focus.apk";
            using (StreamReader sr = File.OpenText(fileName))
            {
                milliSeconds = sr.ReadToEnd();
            }
            StartTimer();
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                MainWindow.timer.Stop();
                (new MainWindow()).Show();
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                String fileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/focus.apk";
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.Write(timeToAlarm.Text.ToString()+"000");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void StartTimer()
        {  
            timer = new Timer(Double.Parse(milliSeconds));
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
            this.Hide();
        }
        public void init()
        {
            try
            {
                String fileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/focus.apk";
                if (!File.Exists(fileName))
                {
                    using (System.IO.StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.Write("5000"); // 5 Seconds
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
