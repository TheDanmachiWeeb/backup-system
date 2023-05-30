using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using BackupSystem.Models;

namespace BackupSystem.Dtos;

public partial class ReportDto
{
    public int ReportId { get; set; }
    public int StationId { get; set; }
    public int ConfigId { get; set; }
    public DateTime ReportTime { get; set; }
    public ulong BackupSize { get; set; }
    public Boolean Success { get; set; }
}
