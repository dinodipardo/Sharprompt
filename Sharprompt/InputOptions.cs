using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sharprompt;

public class InputOptions<T>
{
    public string Message { get; set; } = null!;

    public string? Placeholder { get; set; }

    public object? DefaultValue { get; set; }

    public DefaultValueTabBehaviour DefaultValueTabBehaviour
    {
        get => _defaultValueTabBehaviour;
        set
        {
            if (value == DefaultValueTabBehaviour.Configuration)
            {
                value = Prompt.DefaultValueTabBehaviour;
            }
            _defaultValueTabBehaviour = value;
        }
    }
    private DefaultValueTabBehaviour _defaultValueTabBehaviour;

    public IList<Func<object?, ValidationResult?>> Validators { get; } = [];

    internal void EnsureOptions()
    {
        ArgumentNullException.ThrowIfNull(Message);
    }
}
