using System;
using System.Collections.Generic;

namespace BackupSystem.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<StationConfiguration> StationConfigurations { get; } = new List<StationConfiguration>();

    public virtual ICollection<Station> Stations { get; } = new List<Station>();
}
