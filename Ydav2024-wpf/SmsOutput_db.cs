using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydav2024_wpf
{
    class sms_output
    {

        [Key]
        public int _id { get; set; }

        private string phone, text, sent, sent_time, delivery, delivery_time;

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public string Sent
        {
            get { return sent; }
            set { sent = value; }
        }

        public string Sent_time
        {
            get { return sent_time; }
            set { sent_time = value; }
        }

        public string Delivery
        {
            get { return delivery; }
            set { delivery = value; }
        }
        public string Delivery_time
        {
            get { return delivery_time; }
            set { delivery_time = value; }
        }

        public sms_output() { }

        public sms_output(string phone, string text)
        {
            this.phone = phone;
            this.text = text;
            this.sent = "нет информации";
            this.sent_time = "нет информации";
            this.delivery = "нет информации";
            this.delivery_time = "нет информации";
        }
    }
}
