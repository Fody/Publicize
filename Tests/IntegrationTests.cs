using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    Assembly assembly;
    string beforeAssemblyPath;
    string afterAssemblyPath;

    public IntegrationTests()
    {
        beforeAssemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory,@"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll"));
#if (!DEBUG)

        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif

        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        using (var moduleDefinition = ModuleDefinition.ReadModule(beforeAssemblyPath))
        {
            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
            };

            weavingTask.Execute();
            moduleDefinition.Write(afterAssemblyPath);
        }

        assembly = Assembly.LoadFile(afterAssemblyPath);
    }


    [Test]
    public void PrivateClass()
    {
        var type = TestMembers("PrivateClass");
        Assert.IsTrue(type.ContainsHideAttribute());
        ValidateMembers(type);
    }
    [Test]
    public void ContainsOnlyOneAttribute()
    {
        var type = assembly.GetType("ClassWithEditorBrowsableAttribute");

        var attributeCount = type.GetCustomAttributes(false).OfType<EditorBrowsableAttribute>().Count();
        Assert.AreEqual(1, attributeCount);
    }

    [Test]
    public void PublicInterface()
    {
        var type = assembly.GetType("PublicInterface");
        Assert.IsFalse(type.ContainsHideAttribute());

        var property = type.GetProperty("Property");
        Assert.IsFalse(property.GetSetMethod().ContainsHideAttribute());
        Assert.IsFalse(property.GetGetMethod().ContainsHideAttribute());

        var method = type.GetMethod("Method");
        Assert.IsFalse(method.ContainsHideAttribute());

        var @event = type.GetEvent("Event");
        Assert.IsFalse(@event.ContainsHideAttribute());
    }

    [Test]
    public void InternalInterface()
    {
        var type = assembly.GetType("InternalInterface");
        Assert.IsTrue(type.ContainsHideAttribute());

        var property = type.GetProperty("Property");
        Assert.IsFalse(property.GetSetMethod().ContainsHideAttribute());
        Assert.IsFalse(property.GetGetMethod().ContainsHideAttribute());

        var method = type.GetMethod("Method");
        Assert.IsFalse(method.ContainsHideAttribute());

        var @event = type.GetEvent("Event");
        Assert.IsFalse(@event.ContainsHideAttribute());
    }


    [Test]
    public void InternalClass()
    {
        var type = TestMembers("InternalClass");
        Assert.IsTrue(type.ContainsHideAttribute());

        ValidateMembers(type);
    }
    [Test]
    public void PublicClass()
    {
        var type = TestMembers("PublicClass");
        Assert.IsFalse(type.ContainsHideAttribute());

        ValidateMembers(type);
    }

    static void ValidateMembers(Type type)
    {

        var constructors = type.GetConstructors();
        foreach (var constructorInfo in constructors)
        {
            Assert.IsTrue(constructorInfo.IsPublic);
        }

        var publicConstructor = constructors.First(x => x.GetParameters().Any(y => y.Name == "public"));
        Assert.IsFalse(publicConstructor.ContainsHideAttribute());
        var internalConstructor = constructors.First(x => x.GetParameters().Any(y => y.Name == "internal"));
        Assert.IsTrue(internalConstructor.ContainsHideAttribute());
        var privateConstructor = constructors.First(x => x.GetParameters().Any(y => y.Name == "private"));
        Assert.IsTrue(privateConstructor.ContainsHideAttribute());

        var publicProperty = type.GetProperty("PublicProperty");
        Assert.IsFalse(publicProperty.GetSetMethod().ContainsHideAttribute());
        Assert.IsFalse(publicProperty.GetGetMethod().ContainsHideAttribute());

        var privateProperty = type.GetProperty("PrivateProperty");
        Assert.IsTrue(privateProperty.GetSetMethod().ContainsHideAttribute());
        Assert.IsTrue(privateProperty.GetGetMethod().ContainsHideAttribute());

        var internalProperty = type.GetProperty("InternalProperty");
        Assert.IsTrue(internalProperty.GetSetMethod().ContainsHideAttribute());
        Assert.IsTrue(internalProperty.GetGetMethod().ContainsHideAttribute());

        var publicMethod = type.GetMethod("PublicMethod");
        Assert.IsFalse(publicMethod.ContainsHideAttribute());

        var privateMethod = type.GetMethod("PrivateMethod");
        Assert.IsTrue(privateMethod.ContainsHideAttribute());

        var internalMethod = type.GetMethod("InternalMethod");
        Assert.IsTrue(internalMethod.ContainsHideAttribute());

        var publicField = type.GetField("PublicField");
        Assert.IsFalse(publicField.ContainsHideAttribute());

        var privateField = type.GetField("PrivateField");
        Assert.IsTrue(privateField.ContainsHideAttribute());

        var internalField = type.GetField("InternalField");
        Assert.IsTrue(internalField.ContainsHideAttribute());

        var publicEvent = type.GetEvent("PublicEvent");
        Assert.IsFalse(publicEvent.GetRemoveMethod().ContainsHideAttribute());
        Assert.IsFalse(publicEvent.GetAddMethod().ContainsHideAttribute());

        var privateEvent = type.GetEvent("PrivateEvent");
        Assert.IsTrue(privateEvent.GetRemoveMethod().ContainsHideAttribute());
        Assert.IsTrue(privateEvent.GetAddMethod().ContainsHideAttribute());

        var internalEvent = type.GetEvent("InternalEvent");
        Assert.IsTrue(internalEvent.GetRemoveMethod().ContainsHideAttribute());
        Assert.IsTrue(internalEvent.GetAddMethod().ContainsHideAttribute());
    }

    Type TestMembers(string typeName)
    {
        //Type type = assembly.GetType(typeName);
        //MemberInfo memberInfo = type.GetMember("PrivateProperty").First();
        //object[] customAttributes = memberInfo.GetCustomAttributes(typeof (EditorBrowsableAttribute), false);
        dynamic instance = assembly.CreateInstance(typeName);
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


#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
#endif

}