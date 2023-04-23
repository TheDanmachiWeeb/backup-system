using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class BackupConfiguration
        
    {
        public int ID { get; set; }
        public List<String> SourcePaths { get; set; }
        public List<String> DestinationPaths { get; set; }
        public BackupType BackupType { get; set; }
        public string LastBackupPath { get; set; }
    }
}
