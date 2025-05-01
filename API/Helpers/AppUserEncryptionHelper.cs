using Wego.Core.Dtos;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Identity;
using Wego.Core.Services.Contract;

namespace Wego.API.Helpers
{
    public class AppUserEncryptionHelper
    {
        private readonly IEncryptionService _encryptionService;

        public AppUserEncryptionHelper(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public async Task EncryptAppUserAsync(AppUser user)
        {
            var plainFields = new List<string?>
            {
                user.DisplayName,
                user.PassportNumber,
                user.DateOfBirth?.ToString("O"),
                user.Nationality,
                user.Gender?.ToString(),
                user.NationalId,
                user.TripPurpose?.ToString(),
                user.SpecialNeeds,
                user.Address
            };

            var encrypted = await _encryptionService.EncryptAsync(plainFields.Select(f => f ?? string.Empty).ToList());

            user.DisplayName = CombineEncrypted(encrypted[0]);
            user.PassportNumber = CombineEncrypted(encrypted[1]);
            user.DateOfBirth = DateTime.TryParse(await DecryptField(encrypted[2]), out var dob) ? dob : null;
            user.Nationality = CombineEncrypted(encrypted[3]);
            user.Gender = Enum.TryParse<Gender>(await DecryptField(encrypted[4]), out var gender) ? gender : null;
            user.NationalId = CombineEncrypted(encrypted[5]);
            user.TripPurpose = Enum.TryParse<TripPurpose>(await DecryptField(encrypted[6]), out var tp) ? tp : null;
            user.SpecialNeeds = CombineEncrypted(encrypted[7]);
            user.Address = CombineEncrypted(encrypted[8]);
        }
        public async Task DecryptAppUserAsync(AppUser user)
        {
            var encryptedFields = new List<string?>
            {
                 user.DisplayName,
                 user.PassportNumber,
                 user.DateOfBirth?.ToString("O"), // already decrypted below
                 user.Nationality,
                 user.Gender?.ToString(),
                 user.NationalId,
                 user.TripPurpose?.ToString(),
                 user.SpecialNeeds,
                 user.Address
            };

            var ciphertexts = encryptedFields
                .Select(ParseEncrypted)
                .ToList();

            var decrypted = await _encryptionService.DecryptAsync(ciphertexts);

            user.DisplayName = decrypted[0];
            user.PassportNumber = decrypted[1];
            user.DateOfBirth = DateTime.TryParse(decrypted[2], out var dob) ? dob : null;
            user.Nationality = decrypted[3];
            user.Gender = Enum.TryParse<Gender>(decrypted[4], out var gender) ? gender : null;
            user.NationalId = decrypted[5];
            user.TripPurpose = Enum.TryParse<TripPurpose>(decrypted[6], out var tp) ? tp : null;
            user.SpecialNeeds = decrypted[7];
            user.Address = decrypted[8];
        }
        private static string CombineEncrypted(CiphertextResult result)
        {
            return $"{result.Ciphertext}|{result.Pad}";
        }
        private static CiphertextResult ParseEncrypted(string? value)
        {
            if (string.IsNullOrEmpty(value) || !value.Contains("|")) return new CiphertextResult();
            var parts = value.Split('|');
            return new CiphertextResult
            {
                Ciphertext = parts[0],
                Pad = int.Parse(parts[1])
            };
        }
        private async Task<string> DecryptField(CiphertextResult field)
        {
            var result = await _encryptionService.DecryptAsync(new List<CiphertextResult> { field });
            return result.FirstOrDefault() ?? string.Empty;
        }
    }
}
