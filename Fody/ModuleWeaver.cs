using System;
using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;
using Publicize.Fody.Attributes;

public partial class ModuleWeaver
{
    public Action<string> LogInfo { get; set; }
    public ModuleDefinition ModuleDefinition{ get; set; }

    public ModuleWeaver()
    {
        LogInfo = s => { };
    } 

    public void Execute()
    {
        FindSystemTypes();
        // disables publicization for entire assembly
        bool isPluginDisabled = ModuleDefinition.Assembly.CustomAttributes.Any(ca => ca.AttributeType.Name == typeof(DisablePublicizeAttribute).Name);
        if (!isPluginDisabled)
        {
            // get assembly level Publicize state (On or Off)
            bool isEnabled = IsPublicizeEnabled(ModuleDefinition.Assembly.CustomAttributes, true);

            foreach (var type in ModuleDefinition.GetTypes())
            {
                ProcessType(type, isEnabled);
            }
        }
    }

    /// <summary>
    /// Retrieves publicization enable state for a type/member
    /// </summary>
    /// <param name="source"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public bool IsPublicizeEnabled(Collection<CustomAttribute> source, bool defaultValue)
    {
        foreach (CustomAttribute attrib in source)
        {
            if (attrib.AttributeType.Name == typeof(PublicizeOnAttribute).Name)
            {
                return true;
            }
            else if (attrib.AttributeType.Name == typeof(PublicizeOffAttribute).Name)
            {
                return false;
            }
        }
        return defaultValue;
    }
}