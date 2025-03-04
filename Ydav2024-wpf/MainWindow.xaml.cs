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

    public partial class MainWindow : Window
    {
        ApplicationContext db; 
        public MainWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();
            
            List<contact> contacts = db.contact.ToList();
            ListViewContact.ItemsSource = contacts;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var phones = PhoneLog.Connect(IpAddress.Text);
            ListViewPhoneInput.ItemsSource = phones.Phones.PhoneList;
        }
        private void Button_Contact(object sender, RoutedEventArgs e)
        {
            var contacts = ContactLog.Connect(IpAddress.Text);
            ListViewContact.ItemsSource = contacts.Contacts.Contact;
            contacts.Contacts.Contact.ForEach((Contact contact) => {
                var con = new contact(contact.Name, contact.PhoneStr);
                db.contact.Add(con);
            });
            db.SaveChanges();

        }
        private void Button_CVS(object sender, RoutedEventArgs e)
        {
        }
    }
}
