namespace SocialPhotoAppMVC.ViewModels
{
    public class AddPhotoToAlbumVM
    {
        public Photo Photo { get; set; }
        public int SelectedAlbumId { get; set; }
        public IEnumerable<Album> UserAlbums { get; set; }
    }
}
