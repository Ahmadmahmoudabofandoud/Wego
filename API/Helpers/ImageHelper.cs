using AutoMapper;

namespace Wego.API.Helpers
{
    public class ImageHelper
    {
        public static async Task<string> UploadImageAsync(IFormFile img, string folder, string name)
        {
            var directoryPath = Path.Combine("wwwroot", "imgs", folder);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, name);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(stream);
            }

            return filePath;
        }
        public static void RemoveImage(string folder, string fileFullName)
        {
            var filePath = Path.Combine($"wwwroot/imgs/{folder}", fileFullName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
