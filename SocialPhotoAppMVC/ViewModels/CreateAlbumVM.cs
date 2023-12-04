using SocialPhotoAppMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class CreateAlbumVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Cover art is required")]
        public IFormFile? CoverArt { get; set; }
        public string UserId { get; set; }
    }
}
