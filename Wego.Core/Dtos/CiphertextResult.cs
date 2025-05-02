using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wego.Core.Dtos
{
    public class CiphertextResult
    {
        [JsonPropertyName("ciphertext")]
        public string Ciphertext { get; set; } = string.Empty;

        public int pad { get; set; }
    }
}
