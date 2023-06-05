using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Models;

public partial class Report
{
    public int ReportId { get; set; }
    public int StationId { get; set; }
    public int ConfigId { get; set; }
    public DateTime ReportTime { get; set; }
    public ulong BackupSize { get; set; }
    public Boolean Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    [ForeignKey("ConfigId")]
    public virtual Configuration Config { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
