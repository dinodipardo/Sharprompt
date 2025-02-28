using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sharprompt;

public class InputOptions<T>
{
    public string Message { get; set; } = null!;

    public string? Placeholder { get; set; }

    public object? DefaultValue { get; set; }

    public DefaultValueTabBehaviour DefaultValueTabBehaviour { get; set; } = Prompt.DefaultValueTabBehaviour;

    public IList<Func<object?, ValidationResult?>> Validators { get; } = [];

    internal void EnsureOptions()
    {
        ArgumentNullException.ThrowIfNull(Message);
    }
}
