using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public MethodReference EditorBrowsableConstructor;
    public TypeDefinition EditorBrowsableStateType;
    public int AdvancedStateConstant;

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        return Enumerable.Empty<string>();
    }

    public void FindSystemTypes()
    {
        var editorBrowsableAttribute = FindType("System.ComponentModel.EditorBrowsableAttribute");
        EditorBrowsableStateType = FindType("System.ComponentModel.EditorBrowsableState");
        EditorBrowsableConstructor = ModuleDefinition.ImportReference(editorBrowsableAttribute.Methods.First(IsDesiredConstructor));
        var fieldDefinition = EditorBrowsableStateType.Fields.First(x => x.Name == "Advanced");
        AdvancedStateConstant = (int)fieldDefinition.Constant;
    }

    static bool IsDesiredConstructor(MethodDefinition method)
    {
        if (!method.IsConstructor)
        {
            return false;
        }

        if (method.Parameters.Count != 1)
        {
            return false;
        }

        return method.Parameters[0].ParameterType.Name == "EditorBrowsableState";
    }
}