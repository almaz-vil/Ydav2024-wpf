using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydav2024_wpf
{
    class config
    {
        [Key]
        public int _id { get; set; }

        private string name, value;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
