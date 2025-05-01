using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Dtos;
using Wego.Core.Models;

namespace Wego.Core.Services.Contract
{
    public interface IEncryptionService
    {
        Task<List<CiphertextResult>> EncryptAsync(List<string> plaintexts);
        Task<List<string>> DecryptAsync(List<CiphertextResult> ciphertexts);
    }
}
