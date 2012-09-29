using System;

class PrivateClass
{

    public PrivateClass ()
    {
        
    }
// ReSharper disable UnusedParameter.Local
    internal PrivateClass(string @internal)
    {
        
    }
    private PrivateClass(int @private)
    {
        
    }
    
    public PrivateClass(bool @public)
    {
        
    }
    // ReSharper restore UnusedParameter.Local
    string PrivateField;
    internal string InternalField;
    public string PublicField;
    string PrivateProperty { get; set; }
    internal string InternalProperty { get; set; }
    public string PublicProperty { get; set; }
    event EventHandler PrivateEvent;
    internal event EventHandler InternalEvent;
    public event EventHandler PublicEvent; 
    void PrivateMethod()
    {
    }
    internal void InternalMethod()
    {
    }
    public void PublicMethod()
    {
    }
}