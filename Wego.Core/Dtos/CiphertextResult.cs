using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wego.Core.Dtos
{
    //Represents an encrypted string
    public class CiphertextResult
    {
        // List
        public string Ciphertext { get; set; } 

        public int pad { get; set; }
    }
}
