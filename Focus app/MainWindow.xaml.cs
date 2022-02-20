using Focus_app.poco;
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
using System.Windows.Threading;

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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Constants.db.addTask(addTaskText.Text);
            loadTasksList();
            addTaskText.Text = "";
        }

        void loadTasksList()
        {
            TasksListDataGrid.ItemsSource = null;
            TasksListDataGrid.ItemsSource = Constants.db.getAllTasks();
            TasksListDataGrid.Columns[2].Visibility = Visibility.Collapsed;
            TasksListDataGrid.Columns[3].Visibility = Visibility.Collapsed;
            
        }
        void loadCurrentTask()
        {
            poco.Task task = Constants.db.getCurrentTask();
            currentWorkingTaskNameLabel.Content = task != null ? task.TaskName : "";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadCurrentTask();
        }

        private void TasksListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentWorkBtn.Visibility = Visibility.Visible;
            TaskDoneBtn.Visibility = Visibility.Visible;
        }

        private void CurrentWorkBtn_Click(object sender, RoutedEventArgs e)
        {
            Constants.db.updateCurrentTask((poco.Task)TasksListDataGrid.SelectedItem);
            CurrentWorkBtn.Visibility = Visibility.Collapsed;
            TaskDoneBtn.Visibility = Visibility.Collapsed;
            loadTasksList();
        }

        private void TaskDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            Constants.db.updateTaskDone((poco.Task)TasksListDataGrid.SelectedItem);
            CurrentWorkBtn.Visibility = Visibility.Collapsed;
            TaskDoneBtn.Visibility = Visibility.Collapsed;
            loadTasksList();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
           TaskEx.Delay(1000).ContinueWith(_ =>
            {
                
            });
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1)};
            timer.Start();
            timer.Tick += (c,args) =>
            {
                timer.Stop();
                loadTasksList();
            };
        }
    }
}