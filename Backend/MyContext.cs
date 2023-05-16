using BackupSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using System.Text.Json;

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
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Define primary keys
            modelBuilder.Entity<Configuration>().HasKey(c => c.ConfigId);
            modelBuilder.Entity<BackupSource>().HasKey(bs => new { bs.ConfigId, bs.SourcePath});
            modelBuilder.Entity<BackupDestination>().HasKey(bd =>new { bd.ConfigId, bd.DestinationPath });
            modelBuilder.Entity<Group>().HasKey(g => g.GroupId);
            modelBuilder.Entity<StationConfiguration>().HasKey(sc => new { sc.StationId, sc.ConfigId });
            modelBuilder.Entity<StationGroup>().HasKey(sg => new { sg.StationId, sg.GroupId });
            modelBuilder.Entity<Station>().HasKey(s => s.StationId);
            modelBuilder.Entity<Report>().HasKey(r => r.ReportId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            modelBuilder.Entity<BackupDestination>()
            .HasOne(bd => bd.Config)
            .WithMany(c => c.BackupDestinations)
            .HasForeignKey(bd => bd.ConfigId);
            modelBuilder.Entity<BackupSource>()
            .HasOne(bs => bs.Config)
            .WithMany(c => c.BackupSources)
            .HasForeignKey(bs => bs.ConfigId);

  

            modelBuilder.Entity<StationConfiguration>()
                .HasOne(sc => sc.Config)
                .WithMany(c => c.StationConfigurations)
                .HasForeignKey(sc => sc.ConfigId);

            modelBuilder.Entity<StationConfiguration>()
                .HasOne(sc => sc.Group)
                .WithMany(g => g.StationConfigurations)
                .HasForeignKey(sc => sc.GroupId);
   

            modelBuilder.Entity<StationConfiguration>()
                .HasOne(sc => sc.Station)
                .WithMany(s => s.StationConfigurations)
                .HasForeignKey(sc => sc.StationId);

            modelBuilder.Entity<StationGroup>()
                .HasKey(sg => new { sg.StationId, sg.GroupId });

            modelBuilder.Entity<StationGroup>()
                .HasOne(sg => sg.Station)
                .WithMany(s => s.StationGroups)
                .HasForeignKey(sg => sg.StationId);

            modelBuilder.Entity<StationGroup>()
                .HasOne(sg => sg.Group)
                .WithMany(g => g.StationGroups)
                .HasForeignKey(sg => sg.GroupId);



            //modelBuilder.Entity<StationConfiguration>().ToTable("StationConfiguration");

            //modelBuilder.Entity<Configuration>()
            //    .HasMany(c => c.StationConfigurations)
            //    .WithOne(sc => sc.Config)
            //    .HasForeignKey(sc => sc.ConfigId);

            //modelBuilder.Entity<Group>()
            //    .HasMany(g => g.StationConfigurations)
            //    .WithOne(sc => sc.Group)
            //    .HasForeignKey(sc => sc.GroupId);


            //modelBuilder.Entity<Configuration>()
            //    .HasMany(c => c.Stations)
            //    .WithMany(s => s.Configurations)
            //    .UsingEntity<StationConfiguration>(
            //            /*"StationConfiguration",*/ // Specify the join table name
            //        j => j.HasOne(sc => sc.Station).WithMany(),
            //        j => j.HasOne(sc => sc.Config).WithMany());

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasKey(sc => new { sc.ConfigId, sc.StationId });

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasOne<Configuration>(sc => sc.Config)
            //    .WithMany(s => s.StationConfigurations)
            //    .HasForeignKey(sc => sc.ConfigId);

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasOne<Station>(sc => sc.Station)
            //    .WithMany(s => s.StationConfigurations)
            //    .HasForeignKey(sc => sc.StationId);

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasKey(sc => new { sc.ConfigId, sc.GroupId, sc.StationId });

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasOne(sc => sc.Config)
            //    .WithMany(c => c.StationConfigurations)
            //    .HasForeignKey(sc => sc.ConfigId);

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasOne(sc => sc.Group)
            //    .WithMany(g => g.StationConfigurations)
            //    .HasForeignKey(sc => sc.GroupId);

            //modelBuilder.Entity<StationConfiguration>()
            //    .HasOne(sc => sc.Station)
            //    .WithMany(s => s.StationConfigurations)
            //    .HasForeignKey(sc => sc.StationId);


            //modelBuilder.Entity<StationConfiguration>()
            //    .HasMany<Group>(sc => sc.Groups)
            //    .WithMany(g => g.StationConfigurations)
            //    .UsingEntity<StationGroup>(
            //        sg => sg.HasOne(sg => sg.Group).WithMany(),
            //        sg => sg.HasOne(sg => sg.Station).WithMany(),
            //        sg => sg.HasKey(sg => new { sg.StationId, sg.GroupId })
            //    );
        }


        // Define foreign key relationships
        //modelBuilder.Entity<BackupSource>()
        //    .HasOne(bs => bs.Config)
        //    .WithMany(c => c.BackupSources)
        //    .HasForeignKey(bs => bs.ConfigId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //modelBuilder.Entity<BackupDestinations>()
        //    .HasOne(bd => bd.Configuration)
        //    .WithMany(c => c.BackupDestinations)
        //    .HasForeignKey(bd => bd.configId)
        //    .OnDelete(DeleteBehavior.Cascade);



    }
}
