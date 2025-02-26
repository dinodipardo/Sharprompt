using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Sharprompt.Internal;

internal static class PropertyMetadataFactory
{
    public static IReadOnlyList<PropertyMetadata> Create<T>(T model, Type[]? attributeFilter = null) where T : notnull
    {
        //return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //                .Where(x => x.CanWrite && x.GetCustomAttribute<BindIgnoreAttribute>() is null)
        //                .Select(x => new PropertyMetadata(model, x))
        //                .OrderBy(x => x.Order)
        //                .ToArray();

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.CanWrite && x.GetCustomAttribute<BindIgnoreAttribute>() is null);

        if (attributeFilter != null)
        {
            properties = properties
            .Where(x => attributeFilter.Any(attr => x.GetCustomAttributes(attr, false).Length != 0));
        }

        return [.. properties.Select(x => new PropertyMetadata(model, x)).OrderBy(x => x.Order)];
    }
}
