using System;

using static Sharprompt.Enums;

namespace Sharprompt;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SkillLevelAttribute : Attribute
{
    public SkillLevelAttribute(SkillLevel level = SkillLevel.None) => SkillLevel = level;

    public Enums.SkillLevel SkillLevel { get; }
}
