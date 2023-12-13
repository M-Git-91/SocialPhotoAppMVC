using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class SearchAlbumVM
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Username { get; set; }
    }
}
