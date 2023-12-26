using System;

namespace NetMapper.Extensions;

public static class EnumExtension
{
    /// <summary>
    ///     Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
    public static TAttribute? GetAttributeOfType<TAttribute>(this Enum enumVal) where TAttribute : Attribute
    {
        var type = enumVal.GetType();
        var memberInfo = type.GetMember(enumVal.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(TAttribute), false);
        return attributes.Length > 0 ? (TAttribute)attributes[0] : null;
    }
}