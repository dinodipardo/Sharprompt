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
    private readonly Optional<T> _defaultValue; //readonly

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
                    offscreenBuffer.WriteHint($"({_defaultValue.Value} - Tab to select) ");
                    break;
                case DefaultValueTabBehaviour.TabToReset:
                    offscreenBuffer.WriteHint($"({_defaultValue.Value} - Tab to delete) ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

                switch(_options.DefaultValueTabBehaviour)
                {
                    case DefaultValueTabBehaviour.TabToSelect:
                    case DefaultValueTabBehaviour.TabToReset:
                        result = default;
                        break;
                    default:
                        result = _defaultValue;
                        break;
                }

                //result = _options.DefaultValueTabBehaviour == DefaultValueTabBehaviour.TabToSelect ? default : _defaultValue;
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
        if (_defaultValue.HasValue)
        {
            switch (_options.DefaultValueTabBehaviour)
            {
                case DefaultValueTabBehaviour.TabToSelect:
                    // In case of tab to select, we need to set the default value as the current value
                    var defaultStringValue = _defaultValue.Value?.ToString() ?? ""; // ?? "" to avoid null reference exception
                    InputBuffer.Clear();
                    foreach (var c in defaultStringValue)
                    {
                        InputBuffer.Insert(c);
                    }
                    break;
                case DefaultValueTabBehaviour.TabToReset:
                    // Use reflection to reset the readonly field _defaultValue to its default
                    var fieldInfo = typeof(InputForm<T>)
                        .GetField("_defaultValue", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    fieldInfo?.SetValue(this, default);
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        return true;
    }
}
