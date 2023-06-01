using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Daemon
{
    public class BackupScheduler
    {
        public async Task ScheduleBackupProcesses()
        {
            Setup setup = new Setup();
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            ApiHandler api = new ApiHandler();
            List<BackupConfiguration> configs = new List<BackupConfiguration>();
            BackupReport report = new BackupReport();

            await scheduler.Start();
            configs = await setup.SetupConfigs();
            if (setup.stationStatus == status.approved || ApiHandler.offline)
            {
                if (configs.Count < 1)
                {
                    Console.WriteLine("There are no configs assigned to your station");
                }

                // Schedule each backup process with a non-null cron expression
                foreach (var config in configs.Where(p => !string.IsNullOrEmpty(p.periodCron)))
                {
                    try
                    {
                        if (config.periodic)
                        {
                            if (config.finished)
                            {
                                await api.MarkConfigAsFinished(config, true);
                                await Console.Out.WriteLineAsync("Im here");
                            }
                            config.finished = false;
                        }
                        if (!config.finished)
                        {
                            // Create a job detail with the backup process information
                            JobDataMap jobDataMap = new JobDataMap();
                            jobDataMap.Put("config", config); // Store the backup process ID as a job data
                            IJobDetail jobDetail = JobBuilder.Create<Backup>()
                                .UsingJobData(jobDataMap)
                                .Build();
                            // Create a cron trigger based on the cron expression
                            ITrigger trigger = TriggerBuilder.Create()
                                .WithCronSchedule(config.periodCron)
                                .Build();
      
                            // Schedule the job with the trigger
                            await scheduler.ScheduleJob(jobDetail, trigger);
                        }
                    }
                    catch (Exception ex)
                    {
                        BackupLogger backupLogger = new BackupLogger();
                        await backupLogger.LogBackup(config, false, 0, ex.Message);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else if (setup.stationStatus == status.waiting)
            {
                while (setup.stationStatus != status.rejected || setup.stationStatus != status.approved)
                {
                    await Task.Delay(10000);
                    Console.Clear();
                    await ScheduleBackupProcesses();
                }
            }
            else { return; }

        }
    }

}
