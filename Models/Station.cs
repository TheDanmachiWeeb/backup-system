using System;
using System.Collections.Generic;

namespace BackupSystem.Models;

public partial class Station
{
    public int StationId { get; set; }

    public string? StationName { get; set; }

    public string? IpAddress { get; set; } 

    public string? MacAddress { get; set; } 

    public bool Active { get; set; }

    public virtual ICollection<Report> Reports { get; } = new List<Report>();

    public virtual ICollection<StationConfiguration> StationConfigurations { get; } = new List<StationConfiguration>();

    public virtual ICollection<Group> Groups { get; } = new List<Group>();
}
