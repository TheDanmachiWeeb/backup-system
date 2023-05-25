
namespace Daemon
{
    using System.IO;
    public class Program
    {
        static void Main(string[] args)
        {

          //Console.WriteLine("starting program");
            BackupScheduler scheduler = new BackupScheduler();
            try
            {
                scheduler.ScheduleBackupProcesses().Wait();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                BackupReport report = new BackupReport();
            }
        }
        
    }
}