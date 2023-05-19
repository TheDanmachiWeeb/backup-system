using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
      public class LogEntry
    {
        public int ConfigId { get; set; }
        public int StationId { get; set; }
        public bool Success { get; set; }
    }
}
