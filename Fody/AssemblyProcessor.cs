using Mono.Cecil;

public class AssemblyProcessor
{
    TypeProcessor typeProcessor;
    ModuleDefinition moduleDefinition;

    public AssemblyProcessor(TypeProcessor typeProcessor, ModuleDefinition moduleDefinition)
    {
        this.typeProcessor = typeProcessor;
        this.moduleDefinition = moduleDefinition;
    }

    public void Execute()
    {
        foreach (var type in moduleDefinition.GetTypes())
        {
            typeProcessor.Execute(type);
        }
    }

}