using System.ComponentModel;
using System.Runtime.CompilerServices;

class ClassWithNested
{
    public class NestedClass
    {
    }

    [CompilerGenerated]
    private sealed class NestedCompilerGenerated { }
}

[EditorBrowsable(EditorBrowsableState.Advanced)]
class ClassWithEditorBrowsableAttribute
{
}