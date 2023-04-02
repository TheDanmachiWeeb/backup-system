using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<StationGroup> StationGroups { get; set; } = new List<StationGroup>();
    public virtual ICollection<StationConfiguration> StationConfigurations { get; set; } = new List<StationConfiguration>();
}
