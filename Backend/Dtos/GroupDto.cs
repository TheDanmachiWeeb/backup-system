using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using BackupSystem.Models;

namespace BackupSystem.Dtos;

public partial class GroupDto
{
    public int GroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<StationDto> Stations { get; set; } = new List<StationDto>();
    public virtual ICollection<ConfigurationDto> Configs { get; set; } = new List<ConfigurationDto>();
}