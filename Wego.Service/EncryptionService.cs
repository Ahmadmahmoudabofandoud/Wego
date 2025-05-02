using System.Net.Http.Json;
using Wego.Core.Dtos;
using Wego.Core.Services.Contract;

namespace Wego.Service
{
    public class EncryptionService(HttpClient httpClient) : IEncryptionService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<CiphertextResult>> EncryptAsync(List<string> plaintexts)
        {
            var request = new PlaintextInput { Plaintext = plaintexts };
            var response = await _httpClient.PostAsJsonAsync("/encrypt", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Encryption service failed: {response.StatusCode} - {errorContent}");
            }
            //throw new Exception("Encryption service failed");


            var result = await response.Content.ReadFromJsonAsync<EncryptionOutput>();
            return result?.Results ?? new List<CiphertextResult>();
        }

        public async Task<List<string>> DecryptAsync(List<CiphertextResult> ciphertexts)
        {
            var request = new CiphertextInput { Ciphertexts = ciphertexts };
            var response = await _httpClient.PostAsJsonAsync("/decrypt", request);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Decryption service failed");

            var result = await response.Content.ReadFromJsonAsync<DecryptionOutput>();
            return result?.Plaintexts ?? new List<string>();
        }
    }
}
