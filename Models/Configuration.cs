using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Models;

public partial class Configuration
{
    public int ConfigId { get; set; }
        
    public string? ConfigName { get; set; }
    public string? BackupType { get; set; } 

    public int Retention { get; set; }

    public int PackageSize { get; set; }

    public string? PeriodCron { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<BackupDestination> BackupDestinations { get; set; } = new List<BackupDestination>();
    public virtual ICollection<BackupSource> BackupSources { get; set; } = new List<BackupSource>();
    public virtual ICollection<StationConfiguration> StationConfigurations { get; set; } = new List<StationConfiguration>();

    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}