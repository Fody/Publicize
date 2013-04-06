using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Scalpel;

[Remove]
public static class AttributeChecker
{
    public static bool ContainsHideAttribute(this ICustomAttributeProvider provider)
    {
        return provider.GetCustomAttributes(false).OfType<EditorBrowsableAttribute>().Any();
    }
}