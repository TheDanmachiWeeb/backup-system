using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class BackupConfiguration
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public BackupType BackupType { get; set; }
        public string LastBackupPath { get; set; }
    }
}
