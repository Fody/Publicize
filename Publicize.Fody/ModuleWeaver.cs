using Fody;

public partial class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        FindSystemTypes();

        foreach (var type in ModuleDefinition.GetTypes())
        {
            ProcessType(type);
        }
    }

    public override bool ShouldCleanReference => true;
}