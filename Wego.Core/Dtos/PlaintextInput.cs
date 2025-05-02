using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Dtos
{
    //Sends data for encryption
    public class PlaintextInput
    {
        public List<string> Plaintext { get; set; } = new();
    }
}
