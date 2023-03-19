using System;
using System.Collections.Generic;

namespace BackupSystem.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int StationId { get; set; }

    public int ConfigId { get; set; }

    public DateTime ReportTime { get; set; }

    public long BackupSize { get; set; }

    public ulong Success { get; set; }

    public virtual Configuration Config { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
