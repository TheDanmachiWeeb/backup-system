using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Models;
public partial class StationConfiguration
{
    public int StationId { get; set; }

    public int? GroupId { get; set; } = null!;

    public int ConfigId { get; set; }

    [ForeignKey("ConfigId")]
    public virtual Configuration Config { get; set; } = null!;

    [ForeignKey("GroupId")]
    public virtual Group? Group { get; set; } = null!;

    [ForeignKey("StationId")]
    public virtual Station Station { get; set; } = null!;
}
