using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public MethodReference EditorBrowsableConstructor;
    public TypeDefinition EditorBrowsableStateType;
    public int AdvancedStateConstant;

    public void FindSystemTypes()
    {
        var refTypes = new List<TypeDefinition>();
        AddAssemblyIfExists("System", refTypes);
        AddAssemblyIfExists("System.Runtime", refTypes);
        var editorBrowsableAttribute = refTypes.First(x => x.Name == "EditorBrowsableAttribute");
        EditorBrowsableStateType = refTypes.First(x => x.Name == "EditorBrowsableState");
        EditorBrowsableConstructor = ModuleDefinition.ImportReference(editorBrowsableAttribute.Methods.First(IsDesiredConstructor));
        var fieldDefinition = EditorBrowsableStateType.Fields.First(x => x.Name == "Advanced");
        AdvancedStateConstant = (int)fieldDefinition.Constant;
    }

    void AddAssemblyIfExists(string name, List<TypeDefinition> refTypes)
    {
        try
        {
            var assembly = ModuleDefinition.AssemblyResolver.Resolve(new AssemblyNameReference(name, null));
            if (assembly == null)
            {
                return;
            }
            var module = assembly.MainModule;
            refTypes.AddRange(module.Types);
            refTypes.AddRange(ResolveExportedTypes(module));
        }
        catch (AssemblyResolutionException)
        {
            LogInfo($"Failed to resolve '{name}'. So skipping its types.");
        }
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

    static IEnumerable<TypeDefinition> ResolveExportedTypes(ModuleDefinition module)
    {
        return module.ExportedTypes
            .Select(exportedType => exportedType.Resolve())
            .Where(typeDefinition => typeDefinition != null);
    }
}