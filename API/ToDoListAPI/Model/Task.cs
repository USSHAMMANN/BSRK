using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDoListAPI.Models;

public partial class Task
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? Timeframe { get; set; }

    public string Priority { get; set; } = null!;

    public bool Done { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
