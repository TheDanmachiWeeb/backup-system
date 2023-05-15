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

namespace Daemon
{
    public class Backup
    {
        public BackupLogger logger = new BackupLogger();
        private BackupConfiguration Config;
        private BackupType type;
        private string backupFolder;
        private DateTime BackupStartTime;
        public Snapshot snapshot;
        private DateTime lastBackupTime;

        private DateTime lastFullBackupTime;


        private FileManager fileManager = new FileManager();




        public void PerformBackup(BackupConfiguration config)
        {
            string filename = "Snapshot_" + config.ID.ToString();
            string directoryname = "Snapshots";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryname, filename);

            if (File.Exists(filePath))
            {
                snapshot = fileManager.ReadSnapshot(config);
                Console.WriteLine(snapshot.Data[0]);
                Console.WriteLine(snapshot.Data[1]);
                lastFullBackupTime = DateTime.Parse(snapshot.Data[0]);
                lastBackupTime = DateTime.Parse(snapshot.Data[0]);
            }

            this.Config = config;
            this.type = config.BackupType;
            List<string> sourcePaths = config.SourcePaths; 
            List<string> destinationPaths = config.DestinationPaths;

            BackupStartTime = DateTime.Now;

            for (int sID = 0; sID < sourcePaths.Count; sID++)
            {
                for (int dID = 0; dID < destinationPaths.Count; dID++)
                {
                    string sourcePath = sourcePaths[sID];

                    string destinationPath = destinationPaths[dID];

                    ProcessBackup(sourcePath, destinationPath, type);

                    logger.LogBackup(config);

                }
                if (type != BackupType.Differential)
                {
                    Config.LastBackupPath = backupFolder;
                } 
            }
        }

        private void ProcessBackup(string sourcePath, string destinationPath, BackupType backupType)
        {
            string LastFullBackup = Config.LastBackupPath;
            string LastBackupPath = Config.LastBackupPath;

            if ((backupType == BackupType.Differential || backupType == BackupType.Incremental) && !Directory.Exists(LastFullBackup))
            {
                Console.WriteLine("Full backup does not exist > I will create one");
                type = BackupType.Full;
                ProcessBackup(sourcePath, destinationPath, BackupType.Full);
                return;
            }

            backupFolder = Path.Combine(destinationPath, backupType +"Backup_" + BackupStartTime.ToString("yyyy-MM-dd-HH-mm-ss"));
            Directory.CreateDirectory(backupFolder);
            string[] filesToBackup = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

            if (backupType == BackupType.Differential) 
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
            else if (backupType == BackupType.Incremental)
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

            Console.WriteLine("{1} backup completed: {0} files backed up.", filesToBackup.Length, backupType);
            
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

            File.Copy(file, backupFilePath, true);
        }
        
        private void CreateSnapshot(BackupConfiguration config)
        {
            DateTime fileLastModified = BackupStartTime;
            string[] info = new string[] { fileLastModified.ToString(), config.ID.ToString()! };
            Snapshot snapshot = new Snapshot(info);
            fileManager.SaveSnapshot(Config, snapshot);
        }
    }
}