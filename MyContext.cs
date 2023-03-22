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
        public DbSet<StationConfiguration> StationConfigurations { get; set; }
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
            modelBuilder.Entity<BackupDestination>()
            .HasKey(d => new { d.ConfigId, d.DestinationPath });

            modelBuilder.Entity<BackupSource>()
    .HasKey(s => new { s.ConfigId, s.SourcePath });

            modelBuilder.Entity<Configuration>()
    .HasMany(c => c.BackupDestinations)
    .WithOne(d => d.Config)
    .HasForeignKey(d => d.ConfigId);
        }
    }

}
