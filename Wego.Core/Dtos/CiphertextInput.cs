using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Dtos
{
    public class CiphertextInput
    {
        public List<CiphertextResult> Ciphertexts { get; set; } = new();
    }
}
