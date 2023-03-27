
namespace Daemon
{
    using System.IO;
    public class Program
    {
        static void Main(string[] args)
        {
 

            Backup MyBackup = new Backup();
            BackupLogger logger = MyBackup.logger;
            BackupReport Report = new BackupReport();
            BackupConfiguration config = new BackupConfiguration();
            config.BackupType = BackupType.Incremental;
            config.SourcePath = "C:\\Users\\dima\\Pictures\\Screenshots";
            config.DestinationPath = "C:\\Users\\dima\\Pictures\\Camera Roll";


            for (int i = 0; i < 5; i++)
            {

                MyBackup.PerformBackup(config);

                Thread.Sleep(1000);



                string file = config.SourcePath + "\\file" + i;
                File.Create(file).Close();
                StreamWriter sw = new StreamWriter(file);
                sw.WriteLine("bruh");
                sw.Close();
                Console.WriteLine(Report.GenerateSummaryReport(logger.LogEntries));

                Thread.Sleep(1000);


            }
        }
    }
}