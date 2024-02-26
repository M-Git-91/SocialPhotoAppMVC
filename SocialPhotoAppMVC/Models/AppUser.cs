using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Drawing;

namespace SocialPhotoAppMVC.Models
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(30)]
        public string NickName { get; set; } = string.Empty;
        public string ProfilePictureURL { get; set; } = "https://res.cloudinary.com/dfqrqfs3a/image/upload/v1707836114/logo/l6otrikrypyituu6ogbl.jpg";
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
