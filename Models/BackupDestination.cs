using System;
using System.Collections.Generic;

namespace BackupSystem.Models;

public partial class BackupDestination
{
    public int ConfigId { get; set; }

    public string DestinationType { get; set; } = null!;

    public string DestinationPath { get; set; } = null!;

    public virtual Configuration Config { get; set; } = null!;
}
