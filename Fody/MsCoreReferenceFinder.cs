using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public MethodReference EditorBrowsableConstructor;
    public TypeDefinition EditorBrowsableStateType;
    public int AdvancedStateConstant;

    public void FindSystemTypes()
    {
        var system = ModuleDefinition.AssemblyResolver.Resolve("System");
        var systemTypes = system.MainModule.Types;
        var editorBrowsableAttribute = systemTypes.FirstOrDefault(x => x.Name == "EditorBrowsableAttribute");

        if (editorBrowsableAttribute == null)
        {
            var systemRuntime = ModuleDefinition.AssemblyResolver.Resolve("System.Runtime");
            var systemRuntimeTypes = systemRuntime.MainModule.Types;
            editorBrowsableAttribute = systemRuntimeTypes.FirstOrDefault(x => x.Name == "EditorBrowsableAttribute");
            if (editorBrowsableAttribute == null)
            {
                throw new WeavingException("Could not find 'EditorBrowsableAttribute'. Searched in 'System' and 'System.Runtime'.");
            }
            EditorBrowsableStateType = systemRuntimeTypes.First(x => x.Name == "EditorBrowsableState");
        }
        else
        {
            EditorBrowsableStateType = systemTypes.First(x => x.Name == "EditorBrowsableState");
        }

        EditorBrowsableConstructor = ModuleDefinition.ImportReference(editorBrowsableAttribute.Methods.First(IsDesiredConstructor));
        var fieldDefinition = EditorBrowsableStateType.Fields.First(x => x.Name == "Advanced");
        AdvancedStateConstant = (int)fieldDefinition.Constant;
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