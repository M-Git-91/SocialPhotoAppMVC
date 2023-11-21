using CloudinaryDotNet.Actions;

namespace SocialPhotoAppMVC.Services
{
    public interface ICloudService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicUrl);


    }
}
