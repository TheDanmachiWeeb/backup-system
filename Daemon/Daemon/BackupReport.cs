using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class BackupReport
    {

        public int stationId { get; set; }
        public int configId { get; set; }
        public string reportTime { get; set; }
        public int backupSize { get; set; }
        public bool success { get; set; }

        public BackupReport GenerateBackupReport(LogEntry logEntry)
        {
            DateTime x = DateTime.Now;
            string time = x.ToString("yyyy-MM-ddTHH:mm:ss");
            BackupReport report = new BackupReport
            {
                configId = logEntry.ConfigId,
                stationId = logEntry.StationId,
                success = logEntry.Success,
                backupSize = 1,
                reportTime = time
            };

            return report;
        }
    }
}
