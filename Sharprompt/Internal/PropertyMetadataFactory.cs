﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sharprompt.Internal;

internal static class PropertyMetadataFactory
{
    public static IReadOnlyList<PropertyMetadata> Create<T>(T model) where T : notnull
    {
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.CanWrite && property.GetCustomAttribute<BindIgnoreAttribute>() is null)
            .Where(property =>
            {
                var skillLevelAttribute = property.GetCustomAttribute<SkillLevelAttribute>();
                return skillLevelAttribute is null || Prompt.SkillLevel >= skillLevelAttribute.SkillLevel;
            })
            .Select(property => new PropertyMetadata(model, property))
            .OrderBy(metadata => metadata.Order)
            .ToArray();

        return properties;
    }
}
