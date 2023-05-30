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
        public long backupSize { get; set; }
        public bool success { get; set; }

        public BackupReport GenerateBackupReport(LogEntry logEntry)
        {
            string time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            BackupReport report = new BackupReport
            {
                configId = logEntry.ConfigId,
                stationId = logEntry.StationId,
                success = logEntry.Success,
                backupSize = logEntry.backupSize,
                reportTime = time
            };

            return report;
        }
    }
}
