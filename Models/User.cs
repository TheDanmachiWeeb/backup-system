using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; } 
}
