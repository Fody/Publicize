using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Publicize.Fody.Attributes;

[PublicizeOff]
public class PublicDisabledClassWithEnabledMethod
{
    [PublicizeOn]
    private void TestMethodToBePublic()
    {

    }

    private void TestMethod2()
    {

    }
}
