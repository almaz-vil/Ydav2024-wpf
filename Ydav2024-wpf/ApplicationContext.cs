using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Ydav2024_wpf
{
    class ApplicationContext: DbContext
    {
        public DbSet<contact> contact { get; set; }

        public ApplicationContext() : base("DefaultConnection") { }
    }
}
