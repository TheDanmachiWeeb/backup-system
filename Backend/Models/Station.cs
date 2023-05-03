using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.Models;

public class Station
{
    public int StationId { get; set; }

    public string? StationName { get; set; }

    public string? IpAddress { get; set; }

    public string? MacAddress { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    public virtual ICollection<StationGroup> StationGroups { get; set; } = new List<StationGroup>();
    public virtual ICollection<StationConfiguration> StationConfigurations { get; set; } = new List<StationConfiguration>();

    //public virtual ICollection<Configuration> Configs { get; set; } = new List<Configuration>();
    //public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}