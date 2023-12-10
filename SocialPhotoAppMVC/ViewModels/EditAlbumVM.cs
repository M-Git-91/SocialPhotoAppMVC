using SocialPhotoAppMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class EditAlbumVM
    {
        public int AlbumId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string CoverArtUrl { get; set; }
        public string AuthorId { get; set; }
        public string CurrentUserId { get; set; }
        [Required(ErrorMessage = "Cover art is required")]
        public IFormFile NewCoverArt { get; set; }
    }
}
