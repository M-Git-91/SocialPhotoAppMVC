using System.ComponentModel.DataAnnotations;

namespace SocialPhotoAppMVC.ViewModels
{
    public class CreateCommentVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment text is required")]
        [MinLength(1)]
        public string Text { get; set; } = string.Empty;
        public string UserId { get; set; }
        public int PhotoId { get; set; }
    }
}
