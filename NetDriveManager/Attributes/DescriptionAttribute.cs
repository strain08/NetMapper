using System;

namespace NetMapper.Attributes;

internal class DescriptionAttribute : Attribute
{
    private readonly string Description;

    public DescriptionAttribute(string description)
    {
        Description = description;
    }

    public string GetDescription()
    {
        return Description;
    }
}