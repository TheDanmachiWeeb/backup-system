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

        public void LogBackup(BackupConfiguration config)
        {
            LogEntry logEntry = new LogEntry
            {
                SourcePaths = config.sources,
                DestinationPaths = config.destinations,
            };

            LogEntries.Add(logEntry);
        }
    }
}
