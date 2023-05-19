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

            //for (int i = 0; i < 3; i++)
            //{
            //    BackupConfiguration config = new BackupConfiguration
            //    {
            //        configId = i + 1,

            //        sources = new List<source>
            //        {
            //            new source { sourcePath = $"SourcePath{i + 1}" }
            //        },
            //        destinations = new List<destination>
            //        {
            //            new destination { destinationPath = $"DestinationPath{i + 1}" }
            //        },

            //        BackupType = BackupType.Full
            //    };

            //    if (i == 1)
            //    {
            //        config.periodCron = "0 */5 * * * ?";
            //    }
            //    if (i == 2)
            //    {
            //        config.periodCron = "0 * * * * ?";
            //    }
            //    if (i == 3)
            //    {
            //        config.periodCron = "0 */15 * * * ?";
            //    }

            //    configs.Add(config);
            //};

                if (configs.Count < 1)
            {
                Console.WriteLine("There are no configs assigned to your station");
            }

            // Schedule each backup process with a non-null cron expression
            foreach (var config in configs.Where(p => !string.IsNullOrEmpty(p.periodCron)))
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
    }

}
