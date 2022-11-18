using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBravo.Infrastructure
{
    public class Fork : GitRepository
    {
        public GitRepository Parent { get; set; }
    }
}
