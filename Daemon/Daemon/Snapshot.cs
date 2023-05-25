using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class Snapshot
    {
        public List<String> Data { get; set; } = new List<String>(); //data[0] = datetime, data[1] = configID 
        public List<Snapshot> Snapshots { get; set; } = new List<Snapshot>();   

        public Snapshot(string[] data)
        {
            Data = new List<string>(data);
        }
    }
}
