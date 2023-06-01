using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Quartz;
using System.Reflection.Metadata.Ecma335;
using System.IO.Compression;

namespace Daemon
{
    public class Backup  : IJob
    {
        public long backupSize;
        public BackupLogger logger = new BackupLogger();
        private BackupConfiguration Config;
        private BackupType type;
        private string backupFolder;
        private DateTime BackupStartTime;
        public Snapshot snapshot;
        private DateTime? lastBackupTime;
        public BackupReport report = new BackupReport();
        private DateTime lastFullBackupTime;
        private FileManager fileManager = new FileManager();
        private bool backupSuccess = true;
        private BackupConfiguration config;

        public async Task Execute(IJobExecutionContext context)
        {
            backupSize = 0;
            config = context.JobDetail.JobDataMap.Get("config") as BackupConfiguration;
            if (!config.finished)
            {
                foreach (var source in config.sources)
                {
                    bool exists = FileOrDirectoryExists(source.path);
                    if (!exists && backupSuccess == true)
                    {
                        backupSuccess = false;
                        await logger.LogBackup(config, backupSuccess, 0, "Bad source");
                        Console.ReadLine();
                    }
                }
                foreach (var destination in config.destinations)
                {

                    bool exists = FileOrDirectoryExists(destination.path);
                    if (!exists && backupSuccess == true)
                    {
                        backupSuccess = false;
                        await logger.LogBackup(config, backupSuccess, 0, "Bad destination");
                        Console.ReadLine();
                    }
                }

                PerformBackup(config);

                if (!config.periodic)
                {
                    config.finished = true;
                    ApiHandler api = new ApiHandler();
                    await api.MarkConfigAsFinished(config);
                }
            }
        }

        public void PerformBackup(BackupConfiguration config)
        {
            string filename = "Snapshot_" + config.configId.ToString();
            string directoryname = "Snapshots";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryname, filename);

            if (File.Exists(filePath))
            {
                snapshot = fileManager.ReadSnapshot(config);
                lastFullBackupTime = DateTime.Parse(snapshot.Data[0]);
                lastBackupTime = DateTime.Parse(snapshot.Data[0]);
                Console.WriteLine($"Last backup time: {lastBackupTime}, with config ID: {snapshot.Data[1]} ");
            }

            this.Config = config;
            this.type = config.BackupType;
            List<source> sourcePaths = config.sources;
            List<destination> destinationPaths = config.destinations;

            BackupStartTime = DateTime.Now;


            for (int sID = 0; sID < sourcePaths.Count; sID++)
            {
                for (int dID = 0; dID < destinationPaths.Count; dID++)
                {
                    string sourcePath = sourcePaths[sID].path;

                    string destinationPath = destinationPaths[dID].path;
                    ProcessBackup(sourcePath, destinationPath, type);
               
                }
                if (type != BackupType.Diff)
                {
                    Config.LastBackupPath = backupFolder;
                }
            }
            if (backupSuccess == true)
            {
                logger.LogBackup(config, backupSuccess, backupSize);
            }
        }

        private void ProcessBackup(string sourcePath, string destinationPath, BackupType backupType)
        {
            try
            {

                string LastFullBackup = Config.LastBackupPath;
                string LastBackupPath = Config.LastBackupPath;

                if ((backupType == BackupType.Diff || backupType == BackupType.Inc) && !Directory.Exists(LastFullBackup) && lastBackupTime == null)
                {
                    Console.WriteLine("Full backup does not exist > I will create one");
                    type = BackupType.Full;
                    ProcessBackup(sourcePath, destinationPath, BackupType.Full);
                    Config.LastBackupPath = destinationPath;
                    return;
                }

                backupFolder = Path.Combine(destinationPath, backupType + "Backup_" + BackupStartTime.ToString("yyyy-MM-dd-HH-mm-ss"));
                Directory.CreateDirectory(backupFolder);



                string[] filesToBackup = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);



                if (backupType == BackupType.Diff)
                {

                    foreach (string file in filesToBackup)
                    {
                        DateTime fileLastModified = File.GetLastWriteTime(file);

                        if (fileLastModified > lastFullBackupTime)
                        {
                            MyCopy(sourcePath, file, backupFolder);
                        }
                    }
                }
                else if (backupType == BackupType.Inc)
                {
                    foreach (string file in filesToBackup)
                    {
                        DateTime fileLastModified = File.GetLastWriteTime(file);

                        if (fileLastModified > lastBackupTime)
                        {
                            MyCopy(sourcePath, file, backupFolder);
                        }
                    }
                    CreateSnapshot(Config);
                }
                else
                {
                    foreach (string file in filesToBackup)
                    {
                        MyCopy(sourcePath, file, backupFolder);
                    }
                    CreateSnapshot(Config);
                }
                if (config.zip == true)
                {
                    ZipFile.CreateFromDirectory(backupFolder, backupFolder + ".zip");
                    backupSize = new System.IO.FileInfo(backupFolder+".zip").Length;
                    Directory.Delete(backupFolder, true);
                }
            }
            catch (Exception ex)
            {
                backupSuccess = false;
                logger.LogBackup(config, backupSuccess, backupSize, ex.Message);
            }
        }
        private void MyCopy(string sourcePath, string file, string backupFolder)
        {
            string relativePath = file.Substring(sourcePath.Length + 1);
            string backupFilePath = Path.Combine(backupFolder, relativePath);
            string backupDirectory = Path.GetDirectoryName(backupFilePath);
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }
            backupSize += new System.IO.FileInfo(file).Length;
            File.Copy(file, backupFilePath, true);
        }
        
        private void CreateSnapshot(BackupConfiguration config)
        {
            DateTime fileLastModified = BackupStartTime;
            string[] info = new string[] { fileLastModified.ToString(), config.configId.ToString()! };
            Snapshot snapshot = new Snapshot(info);
            fileManager.SaveSnapshot(Config, snapshot);
        }

        private bool FileOrDirectoryExists(string name)
        {
            return (Directory.Exists(name) || File.Exists(name));
        }
    }
}