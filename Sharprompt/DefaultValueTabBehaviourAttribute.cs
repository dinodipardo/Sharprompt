using System;

namespace Sharprompt;

[AttributeUsage(AttributeTargets.Property)]
public sealed class DefaultValueTabBehaviourAttribute : Attribute
{
    public DefaultValueTabBehaviourAttribute(DefaultValueTabBehaviour tabBehaviour = DefaultValueTabBehaviour.Configuration) => TabBehaviour = tabBehaviour;

    public DefaultValueTabBehaviour TabBehaviour { get; }
}
