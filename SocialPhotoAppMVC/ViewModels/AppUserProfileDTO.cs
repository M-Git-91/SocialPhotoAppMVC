using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace SocialPhotoAppMVC.ViewModels
{
    public class AppUserProfileDTO
    {
        public string UserId { get; set; }
        public string NickName { get; set; } = string.Empty;
        public string ProfilePictureURL { get; set; } = "https://res.cloudinary.com/dfqrqfs3a/image/upload/v1697360852/logo/nbhvsn7pmjzh5wumus1e.png";
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; }
        public IPagedList<Photo>? Photos { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
