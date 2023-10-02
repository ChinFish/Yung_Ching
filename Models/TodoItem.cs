using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TodoItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsComplete { get; set; }
}
