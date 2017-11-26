using System;

public interface PublicInterface
{
    string Property { get; set; }
    void Method();
    event EventHandler Event;
}