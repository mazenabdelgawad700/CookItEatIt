using Microsoft.AspNetCore.Http;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class FileService() : IFileService
    {

        public async Task<ReturnBase<string>> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            ArgumentNullException.ThrowIfNull(imageFile);

            string contentPath = "D:\\Sites\\site21841\\wwwroot\\FileStorage";
            string uploadsFolder = "Uploads";
            string path = Path.Combine(contentPath, uploadsFolder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string ext = Path.GetExtension(imageFile.FileName);
            if (!allowedFileExtensions.Contains(ext))
            {
                return ReturnBaseHandler.Failed<string>($"Only {string.Join(", ", allowedFileExtensions)} are allowed.");
            }

            string fileName = $"{Guid.NewGuid()}{ext}";
            string fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);

            await imageFile.CopyToAsync(stream);

            return ReturnBaseHandler.Success(fileNameWithPath, "Picture Saved Successfully");
        }
        public ReturnBase<bool> DeleteFile(string fileNameWithExtension)
        {
            if (string.IsNullOrEmpty(fileNameWithExtension))
                return ReturnBaseHandler.Failed<bool>($"file({nameof(fileNameWithExtension)}) is null");

            string contentPath = "D:\\Sites\\site21841\\wwwroot\\FileStorage";
            string path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

            if (!File.Exists(path))
                return ReturnBaseHandler.Failed<bool>($"Invalid file {path}");

            File.Delete(path);
            return ReturnBaseHandler.Deleted<bool>("Picture Deleted Successfully");
        }
    }
}