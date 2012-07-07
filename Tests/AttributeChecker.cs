using System.ComponentModel;
using System.Reflection;

public static class AttributeChecker
{
    public static bool ContainsHideAttribute(this ICustomAttributeProvider provider)
    {
        foreach (var customAttribute in provider.GetCustomAttributes(false))
        {
            if (customAttribute is EditorBrowsableAttribute)
            {
                return true;
            }
        }
        return false;
    }
}