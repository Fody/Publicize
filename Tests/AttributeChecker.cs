using System.ComponentModel;
using System.Linq;
using System.Reflection;

public static class AttributeChecker
{
    public static bool ContainsHideAttribute(this ICustomAttributeProvider provider) => provider.GetCustomAttributes(false).OfType<EditorBrowsableAttribute>().Any();
}