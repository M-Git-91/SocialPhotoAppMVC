using CloudinaryDotNet.Actions;

namespace SocialPhotoAppMVC.Services
{
    public interface ICloudService
    {
        ImageUploadResult AddPhoto(IFormFile file);
        DeletionResult DeletePhoto(string publicId);

    }
}
