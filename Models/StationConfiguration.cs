using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Models;
public partial class StationConfiguration
{
    public int StationId { get; set; }

    public int? GroupId { get; set; }

    public int ConfigId { get; set; }

    public virtual Configuration Config { get; set; } = null!;

    public virtual Group? Group { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
