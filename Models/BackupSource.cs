using System;
using System.Collections.Generic;

namespace BackupSystem.Models;

public partial class BackupSource
{
    public int ConfigId { get; set; }

    public string SourcePath { get; set; } = null!;

    public virtual Configuration Config { get; set; } = null!;
}
