using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class Station
    {
        public string name { get; set; }
        public string macAdress { get; set; }

        public string getStationName()
        {
            return name;
        }
        public string getStationMac() 
        { 
            return macAdress; 
        }
    }

}
