using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydav2024_wpf
{
    class sms_input
    {
        [Key]
        public int _id { get; set; }

        private string time, phone, id_na_android, body;

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
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public sms_input() { }

        public sms_input(string time, string phone, string id_na_android, string body)
        {
            this.id_na_android = id_na_android;
            this.time = time;
            this.phone = phone;
            this.body = body;
        }

    }
}
