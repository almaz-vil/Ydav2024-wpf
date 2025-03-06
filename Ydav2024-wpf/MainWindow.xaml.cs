using System;
using System.IO;
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
            ComboBoxContact.ItemsSource = contacts;
            ComboBoxContact.DataContext = contacts;
            
            List<phone_input> phoneDb = db.phone_input.ToList();
            ListViewPhoneInput.ItemsSource = phoneDb;
            
            List<sms_input> smsInputDb = db.sms_input.ToList();
            ListViewSmsInput.ItemsSource = smsInputDb;

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
        private void Button_Contact(object sender, RoutedEventArgs e)
        {
            var contacts = ContactLog.Connect(IpAddress.Text);
            contacts.Contacts.Contact.ForEach((Contact contact) => {
                var con = new contact(contact.Name, contact.PhoneStr);
                db.contact.Add(con);
            });
            db.SaveChanges();

            List<contact> contactsDb = db.contact.ToList();
            ListViewContact.ItemsSource = contactsDb;


        }
        private void Button_CVS(object sender, RoutedEventArgs e)
        {
            var date = new DateTime();
           
            // Настройка диалогового окна сохранения файла 
            var dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.FileName = "Контакты " + date.ToString("dd-MM-YYYY");  // Имя файла по умолчанию
            dialog.DefaultExt = ".csv";  // Расширение файла по умолчанию
            dialog.Filter = "Text documents (.csv)|*.csv";  // Фильтр файлов по расширению

            // Отображение диалогового окна сохранения файла
            bool? result = dialog.ShowDialog();

            // Обработка результатов диалогового окна
            if (result == true)
            {  // Сохранение документа
                string filename = dialog.FileName;
                List<contact> contactsDb = db.contact.ToList();
                StringBuilder stringBuilder = new StringBuilder("");
                contactsDb.ForEach((contact contact) =>
                {
                    stringBuilder.Append(contact.Name + ", " + contact.Phone + "\r\n");
                });
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine(stringBuilder);
                }
            }

        }


        private void Button_Delete_Contact(object sender, RoutedEventArgs e)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM contacts; DELETE FROM sqlite_sequence WHERE name='contacts';");
            db.SaveChanges();
            List<contact> contactsDb = db.contact.ToList();
            ListViewContact.ItemsSource = contactsDb;

        }

        private void Button_Input_Phone(object sender, RoutedEventArgs e)
        {
            StringBuilder param = new StringBuilder();
            var phones = PhoneLog.Connect(IpAddress.Text);
            phones.Phones.PhoneList.ForEach((Phone phone) =>
            {
                var ph = new phone_input(phone.Time, phone.PhoneName, phone.Id, phone.Status);
                if (param.Length > 0)
                {
                    param.Append(" OR ");

                }
                param.Append("_id=" + phone.Id);
                db.phone_input.Add(ph);
            });
            db.SaveChanges();

            var phoneDelete = PhoneDelete.Connect(IpAddress.Text, param.ToString());

            List<phone_input> phoneDb = db.phone_input.ToList();
            ListViewPhoneInput.ItemsSource = phoneDb;

        }

        private void Button_Input_Sms(object sender, RoutedEventArgs e)
        {
            StringBuilder param = new StringBuilder();
            var smsInput = SMSInputLog.Connect(IpAddress.Text);
            smsInput.SmsInput.Sms.ForEach((Sms sms) =>
            {
                var sm = new sms_input(sms.Time, sms.Phone, sms.Id, sms.Body);
                if (param.Length > 0)
                {
                    param.Append(" OR ");

                }
                param.Append("_id=" + sms.Id);
                db.sms_input.Add(sm);
            });
            db.SaveChanges();

            var smsInputDelete = SMSInputDelete.Connect(IpAddress.Text, param.ToString());

            List<sms_input> smsDb = db.sms_input.ToList();
            ListViewSmsInput.ItemsSource = smsDb;

        }
    }
}
