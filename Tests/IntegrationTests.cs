using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using TUnit.Core;
using Fody;
using TestResult = Fody.TestResult;
using System.Threading.Tasks;

// every test weaves into the same fodytemp folder
[NotInParallel]
public class IntegrationTests
{
    TestResult testResult;

    public IntegrationTests()
    {
        var weaver = new ModuleWeaver
        {
            Config = XElement.Parse("""<Publicize IncludeCompilerGenerated="true" />""")
        };
        testResult = weaver.ExecuteTestRun("AssemblyToProcess.dll");
    }

    [Test]
    public async Task PrivateClass()
    {
        var type = TestMembers("PrivateClass");
        await Assert.That(type.ContainsHideAttribute()).IsTrue();
        await ValidateMembers(type);
    }

    [Test]
    public async Task ContainsOnlyOneAttribute()
    {
        var type = testResult.Assembly.GetType("ClassWithEditorBrowsableAttribute");

        var attributeCount = type.GetCustomAttributes(false).OfType<EditorBrowsableAttribute>().Count();
        await Assert.That(attributeCount).IsEqualTo(1);
    }

    [Test]
    public async Task PublicInterface()
    {
        var type = testResult.Assembly.GetType("PublicInterface");
        await Assert.That(type.ContainsHideAttribute()).IsFalse();

        var property = type.GetProperty("Property");
        await Assert.That(property.GetSetMethod().ContainsHideAttribute()).IsFalse();
        await Assert.That(property.GetGetMethod().ContainsHideAttribute()).IsFalse();

        var method = type.GetMethod("Method");
        await Assert.That(method.ContainsHideAttribute()).IsFalse();

        var @event = type.GetEvent("Event");
        await Assert.That(@event.ContainsHideAttribute()).IsFalse();
    }

    [Test]
    public async Task InternalInterface()
    {
        var type = testResult.Assembly.GetType("InternalInterface");
        await Assert.That(type.ContainsHideAttribute()).IsTrue();

        var property = type.GetProperty("Property");
        await Assert.That(property.GetSetMethod().ContainsHideAttribute()).IsFalse();
        await Assert.That(property.GetGetMethod().ContainsHideAttribute()).IsFalse();

        var method = type.GetMethod("Method");
        await Assert.That(method.ContainsHideAttribute()).IsFalse();

        var @event = type.GetEvent("Event");
        await Assert.That(@event.ContainsHideAttribute()).IsFalse();
    }

    [Test]
    public async Task InternalClass()
    {
        var type = TestMembers("InternalClass");
        await Assert.That(type.ContainsHideAttribute()).IsTrue();

        await ValidateMembers(type);
    }

    [Test]
    public async Task NestedCompilerGeneratedClass()
    {
        var type = (Type)testResult.GetInstance("ClassWithNested+NestedCompilerGeneratedClass")
            .GetType();
        await Assert.That(type.ContainsHideAttribute()).IsTrue();
        await Assert.That(type.IsNestedPublic).IsTrue();
    }

    [Test]
    public async Task PublicClass()
    {
        var type = TestMembers("PublicClass");
        await Assert.That(type.ContainsHideAttribute()).IsFalse();

        await ValidateMembers(type);
    }

    static async Task ValidateMembers(Type type)
    {
        var constructors = type.GetConstructors();
        foreach (var constructorInfo in constructors)
        {
            await Assert.That(constructorInfo.IsPublic).IsTrue();
        }

        var publicConstructor = constructors.First(_ => _.GetParameters().Any(y => y.Name == "public"));
        await Assert.That(publicConstructor.ContainsHideAttribute()).IsFalse();
        var internalConstructor = constructors.First(_ => _.GetParameters().Any(y => y.Name == "internal"));
        await Assert.That(internalConstructor.ContainsHideAttribute()).IsTrue();
        var privateConstructor = constructors.First(_ => _.GetParameters().Any(y => y.Name == "private"));
        await Assert.That(privateConstructor.ContainsHideAttribute()).IsTrue();

        var publicProperty = type.GetProperty("PublicProperty");
        await Assert.That(publicProperty.GetSetMethod().ContainsHideAttribute()).IsFalse();
        await Assert.That(publicProperty.GetGetMethod().ContainsHideAttribute()).IsFalse();

        var privateProperty = type.GetProperty("PrivateProperty");
        await Assert.That(privateProperty.GetSetMethod().ContainsHideAttribute()).IsTrue();
        await Assert.That(privateProperty.GetGetMethod().ContainsHideAttribute()).IsTrue();

        var internalProperty = type.GetProperty("InternalProperty");
        await Assert.That(internalProperty.GetSetMethod().ContainsHideAttribute()).IsTrue();
        await Assert.That(internalProperty.GetGetMethod().ContainsHideAttribute()).IsTrue();

        var publicMethod = type.GetMethod("PublicMethod");
        await Assert.That(publicMethod.ContainsHideAttribute()).IsFalse();

        var privateMethod = type.GetMethod("PrivateMethod");
        await Assert.That(privateMethod.ContainsHideAttribute()).IsTrue();

        var internalMethod = type.GetMethod("InternalMethod");
        await Assert.That(internalMethod.ContainsHideAttribute()).IsTrue();

        var publicField = type.GetField("PublicField");
        await Assert.That(publicField.ContainsHideAttribute()).IsFalse();

        var privateField = type.GetField("PrivateField");
        await Assert.That(privateField.ContainsHideAttribute()).IsTrue();

        var internalField = type.GetField("InternalField");
        await Assert.That(internalField.ContainsHideAttribute()).IsTrue();

        var publicEvent = type.GetEvent("PublicEvent");
        await Assert.That(publicEvent.GetRemoveMethod().ContainsHideAttribute()).IsFalse();
        await Assert.That(publicEvent.GetAddMethod().ContainsHideAttribute()).IsFalse();

        var privateEvent = type.GetEvent("PrivateEvent");
        await Assert.That(privateEvent.GetRemoveMethod().ContainsHideAttribute()).IsTrue();
        await Assert.That(privateEvent.GetAddMethod().ContainsHideAttribute()).IsTrue();

        var internalEvent = type.GetEvent("InternalEvent");
        await Assert.That(internalEvent.GetRemoveMethod().ContainsHideAttribute()).IsTrue();
        await Assert.That(internalEvent.GetAddMethod().ContainsHideAttribute()).IsTrue();
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