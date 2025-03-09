using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ydav2024_wpf
{
    class contact
    {
        [Key]
        public int _id { get; set; }

        private string name, phone;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }


        public contact() { }

        public contact(string name, string phone)
        {
            this.name = name;
            this.phone = phone;
        }
    }
}
