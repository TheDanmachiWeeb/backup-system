
namespace Daemon
{
    using System.IO;
    public class Program
    {
        static void Main(string[] args)
        {

            //List<string> sources = new List<string> { "C:\\Users\\dima\\Pictures\\Source3", "C:\\Users\\dima\\Pictures\\Source2", "C:\\Users\\dima\\Pictures\\Source1" };
            //List<string> destinations = new List<string> { "C:\\Users\\dima\\Pictures\\Destination1", "C:\\Users\\dima\\Pictures\\Destination2" };

            // List<string> sources = new List<string> { "C:\\Users\\dima\\Pictures\\Screenshots" };
            // List<string> destinations = new List<string> { "C:\\Users\\dima\\Pictures\\Camera Roll" };
            //FileManager manager = new FileManager();
            //Backup MyBackup = new Backup();
            //BackupLogger logger = MyBackup.logger;
            //BackupReport Report = new BackupReport();
            //BackupConfiguration config = new BackupConfiguration();

            //List<source> sources = new List<source>()
            //    {
            //        new source() { sourcePath = "C:\\Users\\dima\\Pictures\\Source1" },
            //        new source() { sourcePath = "C:\\Users\\dima\\Pictures\\Source2" },
            //        new source() { sourcePath = "C:\\Users\\dima\\Pictures\\Source3" }
            //    };
            
            //List<destination> destinations = new List<destination>()
            //    {
            //        new destination() { destinationPath = "C:\\Users\\dima\\Pictures\\Destination1" },
            //        new destination() { destinationPath = "C:\\Users\\dima\\Pictures\\Destination2" }          
            //    };


            //config.BackupType = BackupType.Inc;
            //config.sources = sources;
            //config.destinations = destinations;
            //config.configId = 4;


            //for (int i = 0; i < 5; i++)
            //{
            //    MyBackup.PerformBackup(config);

            //    string file = config.sources[0].sourcePath + "\\file" + "10" + i * 10;
            //    File.Create(file).Close();
            //    StreamWriter sw = new StreamWriter(file);
            //    sw.WriteLine("bruh");
            //    sw.Close();
            //    Console.WriteLine(Report.GenerateBackupReport(logger.LogEntries));

            //    Console.ReadLine();
            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    manager.CheckIDFile();
            //    Console.WriteLine("file test");
            //}


            //FileManager manager = new FileManager();
            //Station station = new Station();
            //ApiHandler api = new ApiHandler();
            //List<BackupConfiguration> configs = new List<BackupConfiguration>();
            //for (int i = 0; i < 5; i++)
            //{
            //    bool file = manager.CheckIDFile();

            //    if (file == false)
            //    {
            //        api.RegisterStation();
            //    }
            //    else
            //    {
            //        string IDString = manager.getID();
            //        if (IDString == "")
            //        {
            //            api.RegisterStation();
            //        }
            //        Console.WriteLine("Your ID in the database: " + IDString);
            //        api.GetConfigsByID(IDString);
            //    }

            //    Console.ReadLine();
            //}
            //
            Console.WriteLine("starting program");
            BackupScheduler scheduler = new BackupScheduler();
            scheduler.ScheduleBackupProcesses().Wait();
            Console.ReadLine();
           }
        
    }
}