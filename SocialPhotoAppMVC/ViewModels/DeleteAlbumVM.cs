namespace SocialPhotoAppMVC.ViewModels
{
    public class DeleteAlbumVM
    { 
        public string UserId { get; set; }
        public string AlbumOwnerId { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
