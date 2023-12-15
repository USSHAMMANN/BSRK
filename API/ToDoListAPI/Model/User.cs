using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ToDoListAPI.Models;

public partial class User
{
    public long Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
