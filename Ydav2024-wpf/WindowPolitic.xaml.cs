using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Ydav2024_wpf
{
    /// <summary>
    /// Логика взаимодействия для WindowPolitic.xaml
    /// </summary>
    public partial class WindowPolitic : Window
    {
        ApplicationContext db;

        public WindowPolitic()
        {
            InitializeComponent();
            string s = File.ReadAllText("politic.html");
            Web.NavigateToString(s);
            db = new ApplicationContext();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var conf = db.config.FirstOrDefault(p => p.Name == "politic");
            if (conf != null)
            {
                conf.Value = "1";
                db.SaveChanges();                
            }
            this.Close();
        }
    }
}
