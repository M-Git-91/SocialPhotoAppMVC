using SocialPhotoAppMVC.Enums;

namespace SocialPhotoAppMVC.ViewModels
{
    public class DeletePhotoVM
    {
        public string PhotoOwnerId { get; set; }
        public string UserId { get; set; }
        public int PhotoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}
