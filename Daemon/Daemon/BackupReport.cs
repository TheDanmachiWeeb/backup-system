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

            bool success = IsReportSuccessful(logEntries);

            var sb = new StringBuilder();
            sb.AppendLine($"Total operations performed: {totalOperations}, ");
            
            return sb.ToString();
        }

        public bool IsReportSuccessful(List<LogEntry> logEntries)
        {
            var groupedEntries = logEntries.GroupBy(entry => entry.ConfigId);

            foreach (var group in groupedEntries)
            {
                bool hasFailure = group.Any(entry => !entry.Success);
                if (hasFailure)
                {
                    return false; // Report has at least one failure
                }
            }

            return true; // All groups have only successful entries
        }

    }
}
