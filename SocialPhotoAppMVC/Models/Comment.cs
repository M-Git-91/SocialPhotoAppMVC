namespace SocialPhotoAppMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public AppUser AppUser { get; set; }
        public Photo Photo { get; set; }
    }
}
