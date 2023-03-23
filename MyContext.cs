using BackupSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection.Emit;

namespace BackupSystem
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<BackupSource> BackupSources { get; set; }
        public DbSet<BackupDestination> BackupDestinations { get; set; }
        public DbSet<StationConfiguration> StationConfiguration { get; set; }
        public DbSet<StationGroup> StationGroup { get; set; }
        public DbSet<Report> Reports { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string? connectionString = Environment.GetEnvironmentVariable("MY_CONNECTION_STRING");

            if (connectionString == null)
                throw new InvalidOperationException("MY_CONNECTION_STRING environment variable is not set.");

            optionsBuilder.UseMySQL(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys
            modelBuilder.Entity<Configuration>().HasKey(c => c.ConfigId);
            modelBuilder.Entity<BackupSource>().HasKey(bs => bs.ConfigId);
            modelBuilder.Entity<BackupDestination>().HasKey(bd => bd.ConfigId);
            modelBuilder.Entity<Group>().HasKey(g => g.GroupId);
            modelBuilder.Entity<StationConfiguration>().HasKey(sc => new { sc.StationId, sc.ConfigId, sc.GroupId });
            modelBuilder.Entity<StationGroup>().HasKey(sg => new { sg.StationId, sg.GroupId });
            modelBuilder.Entity<Station>().HasKey(s => s.StationId);
            modelBuilder.Entity<Report>().HasKey(r => r.ReportId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            //// Define foreign key relationships
            //modelBuilder.Entity<BackupSources>()
            //    .HasOne(bs => bs.Configuration)
            //    .WithMany(c => c.BackupSources)
            //    .HasForeignKey(bs => bs.configId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<BackupDestinations>()
            //    .HasOne(bd => bd.Configuration)
            //    .WithMany(c => c.BackupDestinations)
            //    .HasForeignKey(bd => bd.configId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasOne(sc => sc.Station)
            //    .WithMany(s => s.StationConfigurations)
            //    .HasForeignKey(sc => sc.stationId);


        }
    }
}
