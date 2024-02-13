using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace SocialPhotoAppMVC.ViewModels
{
    public class AppUserProfileVM
    {
        public string UserId { get; set; }
        public string NickName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePictureURL { get; set; } = "https://res.cloudinary.com/dfqrqfs3a/image/upload/v1707836114/logo/l6otrikrypyituu6ogbl.jpg";
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; }
        public IPagedList<Photo>? Photos { get; set; }
        public IPagedList<Album>? Albums { get; set; }
    }
}
