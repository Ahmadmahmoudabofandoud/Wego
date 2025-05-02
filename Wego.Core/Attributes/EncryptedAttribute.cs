using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Attributes
{
    //Marks a property as requiring encryption
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedAttribute : Attribute
    {

    }
}
