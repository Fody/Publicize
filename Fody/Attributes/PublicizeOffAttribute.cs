using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publicize.Fody.Attributes
{ 
    /// <summary>
    /// Turns Publicize Off for this type/member and types below (cascading).
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Struct)]
    public class PublicizeOffAttribute : Attribute
    {
    }
}
