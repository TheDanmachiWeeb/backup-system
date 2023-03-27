using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class BackupReport
    {
        public string GenerateBackupReport(List<LogEntry> logEntries)
        {
            string totalOperations = logEntries.Count.ToString();

            var sb = new StringBuilder();
            sb.AppendLine($"Total operations performed: {totalOperations}");

            return sb.ToString();
        }
    }
}
