using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Models;

public partial class Login
{
    [Key]
    public int LoginId { get; set; }
    [ForeignKey(nameof(User))]
    public string Username { get; set; }

    public string Password { get; set; }

    public virtual User User { get; set; }
}
