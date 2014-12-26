using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publicize.Fody.Attributes
{
    /// <summary>
    /// Disables Publicize for the entire assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class DisablePublicizeAttribute: Attribute
    {
    }
}
