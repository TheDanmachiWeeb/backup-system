using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.Models;

public partial class Configuration
{
    public int ConfigId { get; set; }

    public string? ConfigName { get; set; }

    public Type BackupType { get; set; } 

    public int Retention { get; set; }

    public int PackageSize { get; set; }

    public string? PeriodCron { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
                                                                                                                                                                     
    public virtual ICollection<BackupDestination> BackupDestinations { get; set; } = new List<BackupDestination>();

    public virtual ICollection<BackupSource> BackupSources { get; set; } = new List<BackupSource>();

    public virtual ICollection<StationConfiguration> StationConfigurations { get; set; } = new List<StationConfiguration>();

    public enum Type
    {
        inc,
        full,
        diff
    }
}
