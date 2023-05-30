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

        public async Task LogBackup(BackupConfiguration config, bool backupSuccess, long backupSize, string exceptionMessage = null)
        {

            LogEntry logEntry = new LogEntry
            {
                ConfigId = config.configId,
                StationId = manager.GetStationID(),
                Success = backupSuccess,
                backupSize = backupSize,
            };

            if (logEntry.Success == false && exceptionMessage != null)
            {
                logEntry.errorMessage = exceptionMessage;
                await Console.Out.WriteLineAsync(exceptionMessage);
            }
            else if (logEntry.Success == false && exceptionMessage == null)
            {
                logEntry.errorMessage = "No error message provided";
            }
            //save entry and generate report from it after retrieving it from txt
            report = report.GenerateBackupReport(logEntry);
            await api.PostReport(report);
        }
    }
}
