using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BackupSystem.Dtos;

public partial class BackupDestinationDto
{
    public string Path { get; set; } = null!;
    public string Type { get; set; } = null!;
}