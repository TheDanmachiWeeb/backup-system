using System;
using System.Collections.Generic;

namespace BackupSystem.Models;

public partial class Configuration
{
    public int ConfigId { get; set; }

    public string BackupType { get; set; } = null!;

    public int Retention { get; set; }

    public int? PackageSize { get; set; }

    public string? PeriodCron { get; set; }

    public virtual ICollection<Report> Reports { get; } = new List<Report>();

    public virtual ICollection<StationConfiguration> StationConfigurations { get; } = new List<StationConfiguration>();
}
