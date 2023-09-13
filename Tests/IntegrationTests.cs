using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Fody;
using Xunit;

public class IntegrationTests
{
    TestResult testResult;

    public IntegrationTests()
    {
        var weavingTask = new ModuleWeaver
        {
            Config = XElement.Parse(@"<Publicize IncludeCompilerGenerated=""true"" />")
        };
        testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll");
    }

    [Fact]
    public void PrivateClass()
    {
        var type = TestMembers("PrivateClass");
        Assert.True(type.ContainsHideAttribute());
        ValidateMembers(type);
    }

    [Fact]
    public void ContainsOnlyOneAttribute()
    {
        var type = testResult.Assembly.GetType("ClassWithEditorBrowsableAttribute");

        var attributeCount = type.GetCustomAttributes(false).OfType<EditorBrowsableAttribute>().Count();
        Assert.Equal(1, attributeCount);
    }

    [Fact]
    public void PublicInterface()
    {
        var type = testResult.Assembly.GetType("PublicInterface");
        Assert.False(type.ContainsHideAttribute());

        var property = type.GetProperty("Property");
        Assert.False(property.GetSetMethod().ContainsHideAttribute());
        Assert.False(property.GetGetMethod().ContainsHideAttribute());

        var method = type.GetMethod("Method");
        Assert.False(method.ContainsHideAttribute());

        var @event = type.GetEvent("Event");
        Assert.False(@event.ContainsHideAttribute());
    }

    [Fact]
    public void InternalInterface()
    {
        var type = testResult.Assembly.GetType("InternalInterface");
        Assert.True(type.ContainsHideAttribute());

        var property = type.GetProperty("Property");
        Assert.False(property.GetSetMethod().ContainsHideAttribute());
        Assert.False(property.GetGetMethod().ContainsHideAttribute());

        var method = type.GetMethod("Method");
        Assert.False(method.ContainsHideAttribute());

        var @event = type.GetEvent("Event");
        Assert.False(@event.ContainsHideAttribute());
    }

    [Fact]
    public void InternalClass()
    {
        var type = TestMembers("InternalClass");
        Assert.True(type.ContainsHideAttribute());

        ValidateMembers(type);
    }

    [Fact]
    public void NestedCompilerGeneratedClass()
    {
        var type = (Type)testResult.GetInstance("ClassWithNested+NestedCompilerGeneratedClass")
            .GetType();
        Assert.True(type.ContainsHideAttribute());
        Assert.True(type.IsNestedPublic);
    }

    [Fact]
    public void PublicClass()
    {
        var type = TestMembers("PublicClass");
        Assert.False(type.ContainsHideAttribute());

        ValidateMembers(type);
    }

    static void ValidateMembers(Type type)
    {
        var constructors = type.GetConstructors();
        foreach (var constructorInfo in constructors)
        {
            Assert.True(constructorInfo.IsPublic);
        }

        var publicConstructor = constructors.First(_ => _.GetParameters().Any(y => y.Name == "public"));
        Assert.False(publicConstructor.ContainsHideAttribute());
        var internalConstructor = constructors.First(_ => _.GetParameters().Any(y => y.Name == "internal"));
        Assert.True(internalConstructor.ContainsHideAttribute());
        var privateConstructor = constructors.First(_ => _.GetParameters().Any(y => y.Name == "private"));
        Assert.True(privateConstructor.ContainsHideAttribute());

        var publicProperty = type.GetProperty("PublicProperty");
        Assert.False(publicProperty.GetSetMethod().ContainsHideAttribute());
        Assert.False(publicProperty.GetGetMethod().ContainsHideAttribute());

        var privateProperty = type.GetProperty("PrivateProperty");
        Assert.True(privateProperty.GetSetMethod().ContainsHideAttribute());
        Assert.True(privateProperty.GetGetMethod().ContainsHideAttribute());

        var internalProperty = type.GetProperty("InternalProperty");
        Assert.True(internalProperty.GetSetMethod().ContainsHideAttribute());
        Assert.True(internalProperty.GetGetMethod().ContainsHideAttribute());

        var publicMethod = type.GetMethod("PublicMethod");
        Assert.False(publicMethod.ContainsHideAttribute());

        var privateMethod = type.GetMethod("PrivateMethod");
        Assert.True(privateMethod.ContainsHideAttribute());

        var internalMethod = type.GetMethod("InternalMethod");
        Assert.True(internalMethod.ContainsHideAttribute());

        var publicField = type.GetField("PublicField");
        Assert.False(publicField.ContainsHideAttribute());

        var privateField = type.GetField("PrivateField");
        Assert.True(privateField.ContainsHideAttribute());

        var internalField = type.GetField("InternalField");
        Assert.True(internalField.ContainsHideAttribute());

        var publicEvent = type.GetEvent("PublicEvent");
        Assert.False(publicEvent.GetRemoveMethod().ContainsHideAttribute());
        Assert.False(publicEvent.GetAddMethod().ContainsHideAttribute());

        var privateEvent = type.GetEvent("PrivateEvent");
        Assert.True(privateEvent.GetRemoveMethod().ContainsHideAttribute());
        Assert.True(privateEvent.GetAddMethod().ContainsHideAttribute());

        var internalEvent = type.GetEvent("InternalEvent");
        Assert.True(internalEvent.GetRemoveMethod().ContainsHideAttribute());
        Assert.True(internalEvent.GetAddMethod().ContainsHideAttribute());
    }

    Type TestMembers(string typeName)
    {
        //Type type = assembly.GetType(typeName);
        //MemberInfo memberInfo = type.GetMember("PrivateProperty").First();
        //object[] customAttributes = memberInfo.GetCustomAttributes(typeof (EditorBrowsableAttribute), false);
        var instance = testResult.GetInstance(typeName);
        instance.PrivateProperty = "Foo";
        instance.InternalProperty = "Foo";
        instance.PublicProperty = "Foo";
        instance.PrivateMethod();
        instance.InternalMethod();
        instance.PublicMethod();
        instance.PrivateField = "Foo";
        instance.InternalField = "Foo";
        instance.PublicField = "Foo";
        return instance.GetType();
    }
}