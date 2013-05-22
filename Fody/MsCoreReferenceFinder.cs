using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;

public partial class ModuleWeaver
{
    public MethodReference EditorBrowsableConstructor;
    public TypeDefinition EditorBrowsableStateType;
    public int AdvancedStateConstant;

    public void FindSystemTypes()
    {
        Collection<TypeDefinition> msCoreTypes;

        var attribyteType =
            GetAttributeType("System", out msCoreTypes) ??
            GetAttributeType("System.Runtime", out msCoreTypes);

        EditorBrowsableConstructor = ModuleDefinition.Import(attribyteType.Methods.First(IsDesiredConstructor));
        EditorBrowsableStateType = msCoreTypes.First(x => x.Name == "EditorBrowsableState");
        var fieldDefinition = EditorBrowsableStateType.Fields.First(x => x.Name == "Advanced");
        AdvancedStateConstant = (int)fieldDefinition.Constant;
    }

    TypeDefinition GetAttributeType(string assemblyName, out Collection<TypeDefinition> msCoreTypes)
    {
        var assemblyDefinition = ModuleDefinition.AssemblyResolver.Resolve(assemblyName);
        msCoreTypes = assemblyDefinition.MainModule.Types;
        return msCoreTypes.FirstOrDefault(x => x.Name == "EditorBrowsableAttribute");
    }

    static bool IsDesiredConstructor(MethodDefinition x)
    {
        if (!x.IsConstructor)
        {
            return false;
        }
        if (x.Parameters.Count != 1)
        {
            return false;
        }
        return x.Parameters[0].ParameterType.Name == "EditorBrowsableState";
    }
}