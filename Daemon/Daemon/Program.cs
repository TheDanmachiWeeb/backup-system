
namespace Daemon
{
    using System.IO;
    public class Program
    {
        static void Main(string[] args)
        {

          //Console.WriteLine("starting program");
            BackupScheduler scheduler = new BackupScheduler();
            scheduler.ScheduleBackupProcesses().Wait();
            Console.ReadLine();
        }
        
    }
}