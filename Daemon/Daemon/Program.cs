
namespace Daemon
{
    using System.IO;
    public class Program
    {
        static void Main(string[] args)
        {

            BackupScheduler scheduler = new BackupScheduler();

            try
            {
                scheduler.ScheduleBackupProcesses().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message) ;
                BackupReport report = new BackupReport();
            }
            Console.ReadLine();
        }
        
    }
}