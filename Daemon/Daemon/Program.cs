﻿
namespace Daemon
{
    using System.IO;
    public class Program
    {
        static void Main(string[] args)
        {
        
           List<string> sources = new List<string> { "C:\\Users\\dima\\Pictures\\Source3", "C:\\Users\\dima\\Pictures\\Source2", "C:\\Users\\dima\\Pictures\\Source1" };
           List<string> destinations = new List<string> { "C:\\Users\\dima\\Pictures\\Destination1", "C:\\Users\\dima\\Pictures\\Destination2" };

            // List<string> sources = new List<string> { "C:\\Users\\dima\\Pictures\\Screenshots" };
            // List<string> destinations = new List<string> { "C:\\Users\\dima\\Pictures\\Camera Roll" };
            FileManager manager = new FileManager();
            Backup MyBackup = new Backup();
            BackupLogger logger = MyBackup.logger;
            BackupReport Report = new BackupReport();
            BackupConfiguration config = new BackupConfiguration();
            config.BackupType = BackupType.Incremental;
            config.SourcePaths = sources;
            config.DestinationPaths = destinations;
            config.ID = 4;


            //for (int i = 0; i < 5; i++)
            //{

            //    MyBackup.PerformBackup(config);


            //    string file = config.SourcePaths[0] + "\\file" + "10" + i * 10;
            //    File.Create(file).Close();
            //    StreamWriter sw = new StreamWriter(file);
            //    sw.WriteLine("bruh");
            //    sw.Close();
            //    Console.WriteLine(Report.GenerateBackupReport(logger.LogEntries));

            //    Thread.Sleep(1000);


            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    manager.CheckIDFile();
            //    Console.WriteLine("file test");
            //}



            for (int i = 0; i < 5; i++)
            {
                Station station = new Station();
                ApiHandler api = new ApiHandler();
                bool file = manager.CheckIDFile();
              

              //  manager.Rollback();
                //Console.ReadLine();
                if (file == true)
                {
                    Console.WriteLine("u WERENT REGISTERED YET");
                    api.RegisterStation();
                }
                else
                {
                    string IDString = manager.getID();
                    if (IDString == null)
                    {
                        api.RegisterStation();
                    }
                    Console.WriteLine("Your ID in the database: " +IDString);
                }

                Console.ReadLine();
            }
        }
        
    }
}