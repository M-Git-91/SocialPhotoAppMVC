using SocialPhotoAppMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class UploadPhotoVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; }

        [Required, Range(0, int.MaxValue, ErrorMessage = "Category is required")]
        public Category Category { get; set; }
        public string UserId { get; set; }
    }
}

