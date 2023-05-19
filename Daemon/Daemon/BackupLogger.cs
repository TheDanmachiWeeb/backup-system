using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class BackupLogger
    {
        public List<LogEntry> LogEntries { get; } = new List<LogEntry>();
        private FileManager manager = new FileManager();

        public void LogBackup(BackupConfiguration config, bool backupSuccess)
        {
            LogEntry logEntry = new LogEntry
            {
                ConfigId = config.configId,
                StationId = manager.GetStationID(),
                Success = backupSuccess,
            };

            LogEntries.Add(logEntry);
        }
    }
}
