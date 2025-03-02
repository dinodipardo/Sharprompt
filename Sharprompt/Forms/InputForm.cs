using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Sharprompt.Internal;
using Sharprompt.Strings;

namespace Sharprompt.Forms;

internal class InputForm<T> : TextFormBase<T>
{
    public InputForm(InputOptions<T> options)
    {
        KeyHandlerMaps.Add(ConsoleKey.Tab, HandleTab);

        options.EnsureOptions();

        _options = options;

        _defaultValue = Optional<T>.Create(options.DefaultValue);
    }

    private readonly InputOptions<T> _options;
    private readonly Optional<T> _defaultValue;

    protected override void InputTemplate(OffscreenBuffer offscreenBuffer)
    {
        offscreenBuffer.WritePrompt(_options.Message);

        if (_defaultValue.HasValue)
        {
            switch (_options.DefaultValueTabBehaviour)
            {
                case DefaultValueTabBehaviour.None:
                    offscreenBuffer.WriteHint($"({_defaultValue.Value}) ");
                    break;
                case DefaultValueTabBehaviour.TabToSelect:
                    offscreenBuffer.WriteHint($"({_defaultValue.Value} <Tab> to select) ");
                    break;
                case DefaultValueTabBehaviour.TabToReset:
                    offscreenBuffer.WriteHint($"({_defaultValue.Value} <Tab> to delete) ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_options.DefaultValueTabBehaviour), _options.DefaultValueTabBehaviour, "Invalid value for argument 'DefaultValueTabBehaviour'");
            }
        }

        if (InputBuffer.Length == 0 && !string.IsNullOrEmpty(_options.Placeholder))
        {
            offscreenBuffer.PushCursor();
            offscreenBuffer.WriteHint(_options.Placeholder);
        }

        offscreenBuffer.WriteInput(InputBuffer);
    }

    protected override void FinishTemplate(OffscreenBuffer offscreenBuffer, T result)
    {
        offscreenBuffer.WriteDone(_options.Message);

        if (result is not null)
        {
            offscreenBuffer.WriteAnswer(result.ToString()!);
        }
    }

    protected override bool HandleEnter([NotNullWhen(true)] out T? result)
    {
        var input = InputBuffer.ToString();

        try
        {
            if (string.IsNullOrEmpty(input))
            {
                if (!TypeHelper<T>.IsNullable && !_defaultValue.HasValue)
                {
                    SetError(Resource.Validation_Required);

                    result = default;

                    return false;
                }

                result = _options.DefaultValueTabBehaviour == DefaultValueTabBehaviour.TabToSelect ? default : _defaultValue;
            }
            else
            {
                result = TypeHelper<T>.ConvertTo(input);
            }

            return TryValidate(result, _options.Validators);
        }
        catch (Exception ex)
        {
            SetError(ex);
        }

        result = default;

        return false;
    }

    protected bool HandleTab(ConsoleKeyInfo keyInfo)
    {
        // Determine the tab behaviour based on configuration or prompt default.
        var tabBehaviour = _options.DefaultValueTabBehaviour == DefaultValueTabBehaviour.Configuration
            ? Prompt.DefaultValueTabBehaviour
            : _options.DefaultValueTabBehaviour;

        // If no default value is set, simply return.
        if (!_defaultValue.HasValue)
        {
            return true;
        }

        switch (tabBehaviour)
        {
            case DefaultValueTabBehaviour.TabToSelect:
                // Set the current input buffer to the default value.
                var defaultString = _defaultValue.Value?.ToString() ?? "";
                InputBuffer.Clear();
                foreach (var ch in defaultString)
                {
                    InputBuffer.Insert(ch);
                }
                break;

            case DefaultValueTabBehaviour.TabToReset:
                // Reset the readonly _defaultValue field via reflection.
                var fieldInfo = typeof(InputForm<T>).GetField("_defaultValue", BindingFlags.Instance | BindingFlags.NonPublic);
                fieldInfo?.SetValue(this, default);
                break;

            default:
                // No action is taken for unspecified behaviours.
                break;
        }

        return true;
    }
}
