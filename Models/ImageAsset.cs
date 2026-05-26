using System.ComponentModel.DataAnnotations;

namespace Journal.Models;
public enum ImageType
{
    Render,
    Image
}
public class ImageAsset
{
    public int Id { get; set; }
    [Display(Name = "Titel")]
    public string ImageName { get; set; } = string.Empty;
    [Display(Name = "Typ")]
    public ImageType Type { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.Now;

    public static async Task<ImageAsset?> CreateFromFile(IFormFile file, string webRootPath, ImageAsset dataFromForm)
    {
        //Extention check
        var ext = Path.GetExtension(file.FileName).ToLower();
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        if (!allowed.Contains(ext)) return null;

        //New name for the image and new path
        var newFileName = $"{Guid.NewGuid()}{ext}";
        var folderPath = Path.Combine(webRootPath, "uploads");
        var fullPath = Path.Combine(folderPath, newFileName);

        //Create a directory if it does not exist
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        // Saving
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Record in database
        return new ImageAsset
        {
            ImageName = dataFromForm.ImageName,
            Type = dataFromForm.Type,           
            FileName = newFileName,
            Url = "/uploads/" + newFileName,
            UploadedAt = DateTime.Now
        };
    }
}