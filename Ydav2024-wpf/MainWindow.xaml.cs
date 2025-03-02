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

        public MainWindow()
        {
            InitializeComponent();

            //  установка таймера

            DispatcherTimer timer;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            DataContext = InfoLog.Connect(IpAddress.Text);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
