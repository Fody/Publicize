using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;

public partial class ModuleWeaver
{

    public void ProcessType(TypeDefinition typeDefinition)
    {
        if (IsCompilerGenerated(typeDefinition.CustomAttributes))
        {
            return;
        }
        if (typeDefinition.IsNotPublic)
        {
            if (typeDefinition.IsNested)
            {
                typeDefinition.IsNestedPublic = true;
            }
            else
            {
                typeDefinition.IsPublic = true;
            }
            AddEditorBrowsableAttribute(typeDefinition.CustomAttributes);
        }
        if (typeDefinition.IsInterface)
        {
            return;
        }

        foreach (var method in typeDefinition.Methods)
        {
            ProcessMethod(method);
        }
        foreach (var field in typeDefinition.Fields)
        {
            ProcessField(field);
        }
    }


    void ProcessField(FieldDefinition field)
    {
        if (IsCompilerGenerated(field.CustomAttributes))
        {
            return;
        }
        if (field.IsPublic)
        {
            return;
        }
        var requiresPublicize = false;
        if (field.IsAssembly)
        {
            field.IsAssembly = false;
            requiresPublicize = true;
        }
        if (field.IsPrivate)
        {
            field.IsPrivate = false;
            requiresPublicize = true;
        }
        if (requiresPublicize)
        {
            field.IsPublic = true;
            AddEditorBrowsableAttribute(field.CustomAttributes);
        }
    }


    static bool IsCompilerGenerated(IEnumerable<CustomAttribute> customAttributes)
    {
        return customAttributes.Any(x=>x.AttributeType.Name == "CompilerGeneratedAttribute");
    }

    void ProcessMethod(MethodDefinition method)
    {
        var requiresPublicize = false;
        if (method.IsPublic)
        {
            return;
        }
        if (method.IsAssembly)
        {
            method.IsAssembly = false;
            requiresPublicize = true;
        }
        if (method.IsHideBySig)
        {
            method.IsHideBySig = false;
            requiresPublicize = true;
        }
        if (method.IsPrivate)
        {
            method.IsPrivate = false;
            requiresPublicize = true;
        }

        if (requiresPublicize)
        {
            method.IsPublic = true;
            AddEditorBrowsableAttribute(method.CustomAttributes);
        }
    }

    void AddEditorBrowsableAttribute(Collection<CustomAttribute> customAttributes)
    {
        if (customAttributes.Any(x=>x.AttributeType.Name == "EditorBrowsableAttribute"))
        {
            return;
        }
        var customAttribute = new CustomAttribute(EditorBrowsableConstructor);
        customAttribute.ConstructorArguments.Add(new CustomAttributeArgument(EditorBrowsableStateType, AdvancedStateConstant));
        customAttributes.Add(customAttribute);
    }
}