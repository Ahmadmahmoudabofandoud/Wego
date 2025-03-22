using Microsoft.AspNetCore.Http;

namespace Wego.API.Helpers
{
    public class ImageHelper
    {
        public static async Task<string> UploadImageAsync(IFormFile img, string folder, string name)
        {
            try
            {
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder);

                // التأكد من وجود المجلد، وإذا لم يكن موجودًا يتم إنشاؤه
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, name);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                return $"/images/{folder}/{name}"; // إرجاع المسار النسبي للرد في الـ API
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء رفع الصورة", ex);
            }
        }


        //public static async Task<string> UploadImageAsync(IFormFile img, string folder, string name)
        //{
        //    string[] ext = { "jpg", "png", "jpeg", "gif", "webp" };
        //    var extension = img.FileName.Split('.').Last().ToLower();
        //    if (!ext.Contains(extension))
        //        return string.Empty;
        //    var fileName = $"{name}.{extension}";
        //    using (FileStream file = new($"wwwroot/imgs/{folder}/{fileName}", FileMode.Create))
        //    {
        //        await img.CopyToAsync(file);
        //    }
        //    return fileName;
        //}

        public static void RemoveImage(string folder, string fileFullName)
        {
            var filePath = Path.Combine($"wwwroot/images/{folder}", fileFullName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
