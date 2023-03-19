using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace BackupSystem
{
    public class MyContext : DbContext
    {
        //public DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string? connectionString = Environment.GetEnvironmentVariable("MY_CONNECTION_STRING");

            if (connectionString == null)
                throw new InvalidOperationException("MY_CONNECTION_STRING environment variable is not set.");

            optionsBuilder.UseMySQL(connectionString);
        }

    }

}
