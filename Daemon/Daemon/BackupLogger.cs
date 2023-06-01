using System;
using System.Collections.Generic;
using System.IO;
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
        private int ThreadCounter = 0;

        public async Task LogBackup(BackupConfiguration config, bool backupSuccess, long backupSize, string exceptionMessage = null)
        {
            string path = manager.programFolder + "\\oldReports";
            if (File.Exists(path))
            {
                string reports = manager.getReports(path);
                List<BackupReport> backupReports = new List<BackupReport>(GetBackupReports(reports, config));
                Console.WriteLine();

                foreach (var rep in backupReports)
                {        
                    await api.PostReport(rep, true);
                }
            }
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
            if (ApiHandler.offline && ThreadCounter < 1)
            {
                ThreadCounter++;
                ApiHandler.offline = false;
                BackupScheduler scheduler = new BackupScheduler();
                await api.GetToken();

            }
        }

        private List<BackupReport> GetBackupReports(string content, BackupConfiguration config)
        {
            List<BackupReport> backupReports = new List<BackupReport>();

            string[] entries = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string entry in entries)
            {
                string[] values = entry.Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 6)
                {
                    BackupReport report = new BackupReport();

                    if (int.TryParse(values[0], out int stationId) &&
                        int.TryParse(values[1], out int configId) &&
                        long.TryParse(values[3], out long backupSize) &&
                        bool.TryParse(values[4], out bool success))
                    {
                        report.stationId = stationId;
                        report.configId = configId;
                        report.reportTime = values[2];
                        report.backupSize = backupSize;
                        report.success = success;
                        report.errorMessage = values[5];

                        if (report.configId == config.configId)
                        {
                            backupReports.Add(report);
                            Console.WriteLine("Old report from " + report.reportTime + "    with config ID: " + report.configId + " retrieved");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid backup report entry: " + entry);
                    }
                }
                if (values.Length == 5)
                {
                    BackupReport report = new BackupReport();

                    if (int.TryParse(values[0], out int stationId) &&
                        int.TryParse(values[1], out int configId) &&
                        long.TryParse(values[3], out long backupSize) &&
                        bool.TryParse(values[4], out bool success))
                    {
                        report.stationId = stationId;
                        report.configId = configId;
                        report.reportTime = values[2];
                        report.backupSize = backupSize;
                        report.success = success;


                        if (report.configId == config.configId)
                        {
                            backupReports.Add(report);
                            Console.WriteLine("Old report from " + report.reportTime + "    with config ID: " + report.configId + " retrieved");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid backup report entry: " + entry);
                    }
                }
            }
            return backupReports;
        }
    }
}
