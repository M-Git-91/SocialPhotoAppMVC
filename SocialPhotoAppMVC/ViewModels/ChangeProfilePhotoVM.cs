using SocialPhotoAppMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class ChangeProfilePhotoVM
    {
        public string CurrentUserId { get; set; }
        public string OldProfileImage { get; set; }
        [Required]
        public IFormFile NewProfileImage { get; set;}
    }
}
