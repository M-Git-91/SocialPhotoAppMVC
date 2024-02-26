using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class CreateCommentVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment text is required")]
        public string Text { get; set; }
        public string UserId { get; set; }
        public int PhotoId { get; set; }
    }
}
