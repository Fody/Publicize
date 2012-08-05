using System;
using Mono.Cecil;

public class ModuleWeaver
{
    public Action<string> LogInfo { get; set; }
    public ModuleDefinition ModuleDefinition{ get; set; }

    public ModuleWeaver()
    {
        LogInfo = s => { };
    } 

    public void Execute()
    {
        var msCoreReferenceFinder = new MsCoreReferenceFinder(this, ModuleDefinition.AssemblyResolver);
        msCoreReferenceFinder.Execute();
        var typeProcessor = new TypeProcessor(msCoreReferenceFinder);
        new AssemblyProcessor(typeProcessor, ModuleDefinition).Execute();
    }

}