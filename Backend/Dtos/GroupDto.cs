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
    public virtual ICollection<int> Stations { get; set; } = new List<int>();
    public virtual ICollection<int> Configs { get; set; } = new List<int>();
}