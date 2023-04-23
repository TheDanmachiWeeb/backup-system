using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BackupSystem.Dtos;

public partial class BackupSourceDto
{
    public string SourcePath { get; set; } = null!;
}