using SocialPhotoAppMVC.Enums;

namespace SocialPhotoAppMVC.ViewModels
{
    public class ChangeProfilePhotoVM
    {
        public string CurrentUserId { get; set; }
        public string OldProfileImage { get; set; }
        public IFormFile NewProfileImage { get; set;}
    }
}
