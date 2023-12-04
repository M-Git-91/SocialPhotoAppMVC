using SocialPhotoAppMVC.Enums;

namespace SocialPhotoAppMVC.ViewModels
{
    public class EditAlbumVM
    {
        public int PhotoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverArtUrl { get; set; }
        public string AuthorId { get; set; }
        public string CurrentUserId { get; set; }
        public IFormFile NewCoverArt { get; set; }
    }
}
