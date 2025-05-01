using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Dtos
{
    public class CiphertextResult
    {
        public string Ciphertext { get; set; } = string.Empty;
        public int Pad { get; set; }
    }
}
