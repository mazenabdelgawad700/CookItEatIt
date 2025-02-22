﻿using Microsoft.AspNetCore.Http;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementaion
{
    internal class FileService() : IFileService
    {

        public async Task<ReturnBase<string>> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            ArgumentNullException.ThrowIfNull(imageFile);

            string contentPath = @"D:\RecipeAppImages\";
            //string contentPath = environment.ContentRootPath;
            string path = Path.Combine(contentPath, "Uploads");

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

            string contentPath = @"D:\RecipeAppImages\";
            //string contentPath = environment.ContentRootPath;
            string path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

            if (!File.Exists(path))
                return ReturnBaseHandler.Failed<bool>($"Invalid file {path}");

            File.Delete(path);
            return ReturnBaseHandler.Deleted<bool>("Picture Deleted Successfully");
        }
    }
}