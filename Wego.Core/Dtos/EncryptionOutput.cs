using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Dtos
{
    //Receives encrypted data
    public class EncryptionOutput
    {
        public List<CiphertextResult> Results { get; set; } = new();
    }
}
