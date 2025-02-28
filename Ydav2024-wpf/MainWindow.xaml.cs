using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net;
using System.Net.Sockets;

namespace Ydav2024_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
 
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            //  установка таймера
           /* timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            */
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var info = InfoLog.Connect(IpAddress.Text);
            InfoLogT infoLog = (InfoLogT)this.Resources["infoLog"];
            PhonesT phoneLog = (PhonesT)this.Resources["phoneLog"];
            BatteryT batteryLog = (BatteryT)this.Resources["batteryLog"];
            infoLog.Json =info.Json;
            phoneLog.Time = info.Info.Time;
            phoneLog.Sms = info.Info.Sms;
            batteryLog.Level = info.Info.Battery.Level;
            batteryLog.Status = info.Info.Battery.Status;
            batteryLog.Temperature = info.Info.Battery.Temperature;
            batteryLog.Charge = info.Info.Battery.Charge;
        }
    }
}
