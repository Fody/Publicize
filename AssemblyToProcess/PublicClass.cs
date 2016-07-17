using System;
// ReSharper disable UnusedMember.Local

public  class PublicClass
{

    public PublicClass ()
    {

    }
// ReSharper disable UnusedParameter.Local
    internal PublicClass(string @internal)
    {

    }
    private PublicClass(int @private)
    {

    }

    public PublicClass(bool @public)
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