using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Models;

public partial class StationGroup
{
    public int StationId { get; set; }
    public int GroupId { get; set; }
    public virtual Group Group { get; set; } = null!;
    public virtual Station Station { get; set; } = null!;
}
