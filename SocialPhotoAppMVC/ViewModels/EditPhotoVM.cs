using SocialPhotoAppMVC.Enums;

namespace SocialPhotoAppMVC.ViewModels
{
    public class EditPhotoVM
    {
        public int PhotoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Category Category { get; set; }
        public string AuthorId { get; set; }
        public string CurrentUserId { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
