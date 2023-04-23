using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using BackupSystem.Models;

namespace BackupSystem.Dtos;

public partial class ConfigurationDto
{
    public int ConfigId { get; set; }
    public string? ConfigName { get; set; }
    public string? BackupType { get; set; }
    public int Retention { get; set; }
    public int PackageSize { get; set; }
    public string? PeriodCron { get; set; }
    public virtual ICollection<BackupDestinationDto> Destinations { get; set; } = new List<BackupDestinationDto>();
    public virtual ICollection<BackupSourceDto> Sources { get; set; } = new List<BackupSourceDto>();
    public virtual ICollection<StationDto> Stations { get; set; } = new List<StationDto>();
    public virtual ICollection<GroupDto> Groups { get; set; } = new List<GroupDto>();
}