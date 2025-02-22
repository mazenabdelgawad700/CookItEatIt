using Microsoft.AspNetCore.Http;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IFileService
    {
        Task<ReturnBase<string>> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
        ReturnBase<bool> DeleteFile(string fileNameWithExtension);
    }
}