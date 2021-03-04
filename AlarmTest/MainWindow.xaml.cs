using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace AlarmTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer;
        SoundPlayer player = new SoundPlayer(Properties.Resources.samsung_galaxy_morning_flower_namobilu_com);
        public ObservableCollection<Alarm> alarmClock = new ObservableCollection<Alarm>();

        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerElapsed;
        }

        delegate void UpdateLable(Label statusLbl, string value);
        void UpdateDataLable(Label statusLbl, string value)
        {
            statusLbl.Content = value;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var c in alarmClock)
            {
                this.Dispatcher.Invoke(() =>
                {
                    DateTime currentTime = DateTime.Now;
                    DateTime userTime = c.DateAndTime;

                    if (currentTime.Hour == userTime.Hour && currentTime.Minute == userTime.Minute && currentTime.Second == userTime.Second)
                    {
                        timer.Stop();
                        c.Status = (AlarmStatus)1;
                        List.Items.Refresh();
                        try
                        {
                            UpdateLable upd = UpdateDataLable;

                            //player.SoundLocation = ConfigurationManager.AppSettings["Path"];
                            player.PlayLooping();
                            Serialization.Serialize(alarmClock);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Alarm userAlarm = new Alarm
            {
                DateAndTime = (DateTime)dateTimePicker.Value
            };
            if (userAlarm.DateAndTime > DateTime.Now)
            {
                timer.Start();
                lblStatus.Content = "Running...";
                alarmClock.Add(userAlarm);

                Serialization.Serialize(alarmClock);
            }
            else
            {
                MessageBox.Show("This time already passed");
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            lblStatus.Content = "Stop";
            player.Stop();
            timer.Start();
        }

        private void btnPostponed_Click(object sender, RoutedEventArgs e)
        {
            Alarm userAlarm = new Alarm();
            userAlarm.DateAndTime = DateTime.Now.AddMinutes(1);
            lblStatus.Content = "Stop";
            player.Stop();
            timer.Start();
            lblStatus.Content = "Postponed for 1 minute";
            alarmClock.Add(userAlarm);
        }

        private void AlarmClock_Loaded(object sender, RoutedEventArgs e)
        {
            string xmlString = ConfigurationManager.AppSettings["XmlPath"];
            if (!String.IsNullOrEmpty(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Alarm>));
                StreamReader reader = new StreamReader(xmlString);
                alarmClock = (ObservableCollection<Alarm>)serializer.Deserialize(reader);
                reader.Close();
            }

            List.ItemsSource = alarmClock;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (List.SelectedItems.Count > 0)
            {
                for (int i = 0; i < List.SelectedItems.Count; i++)
                {
                    Alarm clocks = (Alarm)List.SelectedItems[i];

                    if (clocks != null)
                    {
                        alarmClock.Remove(clocks);

                        Serialization.Serialize(alarmClock);
                    }
                }
            }
        }
    }
}
