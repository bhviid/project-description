using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBravo.Infrastructure
{
    public class Fork
    {
        public int Id { get; set; }
        public Author Owner { get; set; }
        public string Name { get; set; }
    }
}
