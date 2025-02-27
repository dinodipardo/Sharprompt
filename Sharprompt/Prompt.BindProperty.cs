using System;
using System.Linq;

using Sharprompt.Forms;
using Sharprompt.Internal;

namespace Sharprompt;

public static partial class Prompt
{
    public static object? BindProperty<T>(T model, string propertyName) where T : notnull
    {
        return StartBindProperty(model, propertyName);
    }

    private static object? StartBindProperty<T>(T model, string propertyName) where T : notnull
    {
        var propertyMetadatas = PropertyMetadataFactory.Create(model);
        var propertyMetadata = propertyMetadatas.FirstOrDefault(x => x.PropertyInfo.Name == propertyName);

        object? result = null;

        if (propertyMetadata is not null)
        {
            var formType = propertyMetadata.DetermineFormType();

            result = formType switch
            {
                FormType.Confirm => MakeConfirm(propertyMetadata),
                FormType.Input => MakeInput(propertyMetadata),
                FormType.List => MakeList(propertyMetadata),
                FormType.MultiSelect => MakeMultiSelect(propertyMetadata),
                FormType.Password => MakePassword(propertyMetadata),
                FormType.Select => MakeSelect(propertyMetadata),
                _ => throw new ArgumentOutOfRangeException()
            };

            propertyMetadata.PropertyInfo.SetValue(model, result);
        }

        return result;
    }
}
