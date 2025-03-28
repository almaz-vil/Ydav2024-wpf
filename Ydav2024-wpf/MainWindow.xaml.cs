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
using System.ComponentModel;

namespace Ydav2024_wpf
{

    public partial class MainWindow : Window
    {
        ApplicationContext db;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new InfoLog();

            db = new ApplicationContext();
            
            List<contact> contacts = db.contact.OrderBy(p=>p.Name).ToList();
            ListViewContact.ItemsSource = contacts;
            ComboBoxContact.ItemsSource = contacts;
            ComboBoxContact.DataContext = contacts;
            
            List<phone_input> phoneDb = db.phone_input.ToList();
            ListViewPhoneInput.ItemsSource = phoneDb;
            
            List<sms_input> smsInputDb = db.sms_input.ToList();
            ListViewSmsInput.ItemsSource = smsInputDb;

            List<sms_output> smsOutputDb = db.sms_output.ToList();
            ListViewSmsOutput.ItemsSource = smsOutputDb;

            var conf = db.config.FirstOrDefault(p => p.Name == "ip");
            if (conf != null)
            {
                IpAddress.Text = conf.Value;
            }
            var politic = db.config.FirstOrDefault(p => p.Name == "politic");
            if (politic != null)
            {
                if (politic.Value == "1")
                {
                    Politic.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Politic.Visibility = Visibility.Visible;
                }
            }
            //  установка таймера

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            BackgroundWorker worker = new BackgroundWorker();

            if (DataContext is InfoLog info)
            {
                if (info.Error != null)
                {
                    TextError.Text = $"Соединение с {IpAddress.Text} ...";
                    DataContext = new InfoLog();
                }
            } else {
                TextError.Text = $"Соединение с {IpAddress.Text} ...";
            }
            worker.DoWork += Worker_DoWork_InfoLog;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted_InfoLog;
            worker.RunWorkerAsync(IpAddress.Text);
        }

        private void Worker_RunWorkerCompleted_InfoLog(object sender, RunWorkerCompletedEventArgs e)
        {
            TextError.Text = "";

            if (e.Result is InfoLog info)
            {
                if (info.Error == null)
                {
                    timer.Start();
                }
                DataContext = info;
            }

        }

        private void Worker_DoWork_InfoLog(object sender, DoWorkEventArgs e)
        {
            var info = InfoLog.Connect(e.Argument as string);
            e.Result = info;
        }

        private void Button_Contact(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            while (timer.IsEnabled)
            {

            }
            DataContext = new InfoLog();
            BackgroundWorker worker = new BackgroundWorker();

           TextError.Text = $"Загрузка контактов. Ждите ...";

            worker.DoWork += Worker_DoWork_Contact ;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted_Contact;
            worker.RunWorkerAsync(IpAddress.Text);


        }

        private void Worker_RunWorkerCompleted_Contact(object sender, RunWorkerCompletedEventArgs e)
        {
            TextError.Text = "";
            if (e.Result is ContactLog info)
            {
                if (info.Error == null)
                {
                    List<contact> contactsDb = db.contact.ToList();
                    ListViewContact.ItemsSource = contactsDb;
                    timer.Start();
                }
            }
            
        }

        private void Worker_DoWork_Contact(object sender, DoWorkEventArgs e)
        {

            var contacts = ContactLog.Connect(e.Argument as string);
            if (contacts.Error == null)
            {
                contacts.Contacts.Contact.ForEach((Contact contact) =>
                {
                    var con = new contact(contact.Name, contact.PhoneStr);
                    db.contact.Add(con);
                });
                db.SaveChanges();
            }
            e.Result = contacts;
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
            timer.Stop();
            while (timer.IsEnabled)
            {

            }
            DataContext = new InfoLog();
            BackgroundWorker worker = new BackgroundWorker();

            TextError.Text = $"Загрузка входящих звонков. Ждите ...";

            worker.DoWork += Worker_DoWork_InputPhone;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted_InputPhone;
            worker.RunWorkerAsync(IpAddress.Text);
        }

        private void Worker_RunWorkerCompleted_InputPhone(object sender, RunWorkerCompletedEventArgs e)
        {
            TextError.Text = "";
            if (e.Result is PhoneLog info)
            {
                if (info.Error == null)
                {
                    List<phone_input> phoneDb = db.phone_input.ToList();
                    ListViewPhoneInput.ItemsSource = phoneDb;
                    timer.Start();
                }
            }

        }

        private void Worker_DoWork_InputPhone(object sender, DoWorkEventArgs e)
        {

            StringBuilder param = new StringBuilder();
            var address = e.Argument as string;
            var phones = PhoneLog.Connect(address);
            if (phones.Error == null)
            {
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
                var phoneDelete = PhoneDelete.Connect(address, param.ToString());
            }
            e.Result = phones;
        }

        private void Button_Input_Sms(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            while (timer.IsEnabled)
            {

            }
            DataContext = new InfoLog();
            BackgroundWorker worker = new BackgroundWorker();

            TextError.Text = $"Загрузка входящих СМС. Ждите ...";

            worker.DoWork += Worker_DoWork_InputSms; ;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted_InputSms; ;
            worker.RunWorkerAsync(IpAddress.Text);
        }

        private void Worker_RunWorkerCompleted_InputSms(object sender, RunWorkerCompletedEventArgs e)
        {
            TextError.Text = "";
            if (e.Result is SMSInputLog info)
            {
                if (info.Error == null)
                {
                    List<sms_input> smsDb = db.sms_input.ToList();
                    ListViewSmsInput.ItemsSource = smsDb;
                    timer.Start();
                }
            }

        }

        private void Worker_DoWork_InputSms(object sender, DoWorkEventArgs e)
        {
            StringBuilder param = new StringBuilder();
            var address = e.Argument as string;
            var smsInput = SMSInputLog.Connect(address);
            if (smsInput.Error == null)
            {
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

                var smsInputDelete = SMSInputDelete.Connect(address, param.ToString());
            }
            e.Result = smsInput;
        }

        private void ComboBoxContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxContact.SelectedItem is contact contactSelect)
            {
                var str = contactSelect.Phone;
                var charsToRemove = new string[] { " ", "-", "(", ")"};
                foreach (var c in charsToRemove)
                {
                    str = str.Replace(c, String.Empty);
                }
                TextBoxPhone.Text = str;
            }
        }

        private void Button_Output_Sms(object sender, RoutedEventArgs e) 
        {
            var smsOutput = new sms_output(TextBoxPhone.Text, TextBoxText.Text);
            db.sms_output.Add(smsOutput);
            db.SaveChanges();
            var smsOutputParam = new SmsOutputParam(smsOutput._id, smsOutput.Phone, smsOutput.Text);
            var (status, json, error) = StatusSMSOutput.Connect(IpAddress.Text, CommandSend.SmsOutput, smsOutputParam.Json());
            List<sms_output> smsOutputDb = db.sms_output.ToList();
            ListViewSmsOutput.ItemsSource = smsOutputDb;

        }

        private void ListViewSmsOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewSmsOutput.SelectedItem is sms_output smsOutput)
            {
                var (status, json, error) = StatusSMSOutput.Connect(IpAddress.Text, CommandSend.SmsOutputStatus, smsOutput._id.ToString());
                var sms = db.sms_output.Find(smsOutput._id);
                sms.Sent = status.Status.Sent.Result;
                sms.Sent_time = status.Status.Sent.Time;
                sms.Delivery = status.Status.Delivery.Result;
                sms.Delivery_time = status.Status.Delivery.Time;
                db.SaveChanges();
                List<sms_output> smsOutputDb = db.sms_output.ToList();
                ListViewSmsOutput.ItemsSource = smsOutputDb;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void ListViewContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewContact.SelectedItem is contact contact)
            {
                var str = contact.Phone;
                var charsToRemove = new string[] { " ", "-", "(", ")" };
                foreach (var c in charsToRemove)
                {
                    str = str.Replace(c, String.Empty);
                }
                TextBoxPhone.Text = str;
                SMSNew.IsSelected = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var conf = db.config.FirstOrDefault(p => p.Name == "ip");
            if (conf != null)
            {
                conf.Value=IpAddress.Text;
                db.SaveChanges();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowPolitic windowPolitic = new WindowPolitic();
            windowPolitic.Owner = this;
            windowPolitic.Show();
            Politic.Visibility = Visibility.Collapsed;
        }
    }
}
