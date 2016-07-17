using System;
// ReSharper disable UnusedMember.Local

internal class InternalClass
{

    public InternalClass ()
    {

    }
// ReSharper disable UnusedParameter.Local
    internal InternalClass(string @internal)
    {

    }
    private InternalClass(int @private)
    {

    }
    public InternalClass(bool @public)
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