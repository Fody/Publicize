using System.ComponentModel;
using System.Runtime.CompilerServices;

class ClassWithNested
{
    public class NestedClass;

    [CompilerGenerated]
    private sealed class NestedCompilerGeneratedClass;
}

[EditorBrowsable(EditorBrowsableState.Advanced)]
class ClassWithEditorBrowsableAttribute;