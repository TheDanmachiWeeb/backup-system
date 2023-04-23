using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BackupSystem.Dtos;

public partial class BackupDestinationDto
{
    public string DestinationPath { get; set; } = null!;
    public string DestinationType { get; set; } = null!;
}