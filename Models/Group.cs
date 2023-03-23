using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<StationConfiguration> StationConfigurations { get; set; } = new List<StationConfiguration>();

    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();
}
