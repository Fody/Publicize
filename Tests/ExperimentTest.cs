using NUnit.Framework;

[TestFixture]
[Ignore]
public class ExperimentTest
{
    [Test]
    public void Simple()
    {
        var weaverHelper = new WeaverHelper(@"AssemblyExperiments\AssemblyExperiments.csproj");
    }
}