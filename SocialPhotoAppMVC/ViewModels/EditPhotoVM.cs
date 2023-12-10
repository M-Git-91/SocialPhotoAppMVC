using SocialPhotoAppMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class EditPhotoVM
    {
        public int PhotoId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public Category Category { get; set; }
        public string AuthorId { get; set; }
        public string CurrentUserId { get; set; }
        [Required(ErrorMessage = "New image is required")]
        public IFormFile NewImage { get; set; }
    }
}
