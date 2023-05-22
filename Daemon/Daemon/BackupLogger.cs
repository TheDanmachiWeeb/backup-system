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
        private BackupReport report = new BackupReport();
        private ApiHandler api = new ApiHandler();

        public async Task LogBackup(BackupConfiguration config, bool backupSuccess)
        {
            LogEntry logEntry = new LogEntry
            {
                ConfigId = config.configId,
                StationId = manager.GetStationID(),
                Success = backupSuccess,
            };
            report = report.GenerateBackupReport(logEntry);
            await api.PostReport(report);

        }
    }
}
