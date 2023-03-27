using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Daemon
{
    public class Backup
    {
        int i = 0;
        public BackupLogger logger = new BackupLogger();
        BackupConfiguration Config;


        public void PerformBackup(BackupConfiguration config)
        {
            this.Config = config;
            List<string> sourcePaths = config.SourcePaths; Console.WriteLine(sourcePaths.Count);
            List<string> destinationPaths = config.DestinationPaths; Console.WriteLine(destinationPaths.Count);
            BackupType backupType = config.BackupType;


            for (int i = 0; i < sourcePaths.Count; i++)
            {
                for (int j = 0; j < destinationPaths.Count; j++)
                {
                    string sourcePath = sourcePaths[i];
                    string destinationPath = destinationPaths[j];

                    switch (backupType)
                    {
                        case BackupType.Full:
                            PerformFullBackup(sourcePath, destinationPath);
                            break;
                        case BackupType.Differential:
                            PerformDifferentialBackup(sourcePath, destinationPath);
                            break;
                        case BackupType.Incremental:
                            PerformIncrementalBackup(sourcePath, destinationPath);
                            break;
                        default:
                            Console.WriteLine("Invalid backup type.");
                            break;    
                    }
                    logger.LogBackup(config);
                }
             
            }


           
        }

        private void PerformFullBackup(string sourcePath, string destinationPath)
        {
            string backupFolder = Path.Combine(destinationPath, "FullBackup_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            Directory.CreateDirectory(backupFolder);

            string[] filesToBackup = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

            foreach (string file in filesToBackup)
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

            Config.LastBackupPath = backupFolder;

            Console.WriteLine("Full backup completed: {0} files backed up.", filesToBackup.Length);
         
        }

        private void PerformDifferentialBackup(string sourcePath, string destinationPath)
        {
            Console.WriteLine(Config.LastBackupPath);
            if (!Directory.Exists(Config.LastBackupPath))
            {
                Console.WriteLine("Full backup does not exist > I will create one");
                PerformFullBackup(sourcePath, destinationPath);
                return;
            }

            string fullBackupDir = Config.LastBackupPath;
            DateTime lastBackupTime = File.GetLastWriteTime(fullBackupDir);
            string backupFolder = Path.Combine(destinationPath, "DifferentialBackup_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            Directory.CreateDirectory(backupFolder);

            string[] filesToBackup = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

            foreach (string file in filesToBackup)
            {
                DateTime fileLastModified = File.GetLastWriteTime(file);

                if (fileLastModified > lastBackupTime)
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
            }

            Console.WriteLine("Differential backup completed: {0} files backed up.", filesToBackup.Length);

        }

        private void PerformIncrementalBackup(string sourcePath, string destinationPath)
        {
            string lastBackupPath = Config.LastBackupPath;

            if (!Directory.Exists(lastBackupPath))
            {
                Console.WriteLine("No bAckup exists > I will create one");
                PerformFullBackup(sourcePath, destinationPath);
                return;
            }
            
            DateTime lastBackupTime = File.GetLastWriteTime(lastBackupPath);
            string backupFolder = Path.Combine(destinationPath, "IncrementalBackup_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            Directory.CreateDirectory(backupFolder);

            string[] filesToBackup = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);

            foreach (string file in filesToBackup)
            {
                DateTime fileLastModified = File.GetLastWriteTime(file);

                if (fileLastModified > lastBackupTime)
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
            }


            Console.WriteLine("Incremental backup completed: {0} files backed up.", filesToBackup.Length);
            Config.LastBackupPath = backupFolder;
        }
    }
}