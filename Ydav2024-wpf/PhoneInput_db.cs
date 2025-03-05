using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ydav2024_wpf
{
    class phone_input
    {
        [Key]
        public int _id { get; set; }

        private string time, phone, id_na_android, status;

        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Id_na_android
        {
            get { return id_na_android; }
            set { id_na_android = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public phone_input() { }
        
        public phone_input(string time, string phone, string id_na_android, string status)
        {
            this.id_na_android = id_na_android;
            this.time = time;
            this.phone = phone;
            this.status = status;
        }

    }
}
