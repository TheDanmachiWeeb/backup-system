using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using BackupSystem.Models;

namespace BackupSystem.Dtos;

public class StationDto
{
    public int StationId { get; set; }
    public string? StationName { get; set; }
    public string? IpAddress { get; set; }
    public string? MacAddress { get; set; }
    public bool Active { get; set; }
    public virtual ICollection<GroupDto> Groups { get; set; } = new List<GroupDto>();
    public virtual ICollection<ConfigurationDto> Configs { get; set; } = new List<ConfigurationDto>();
}