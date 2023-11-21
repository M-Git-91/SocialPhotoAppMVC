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
        public string ProfilePictureURL { get; set; } = "https://res.cloudinary.com/dfqrqfs3a/image/upload/v1697360852/logo/nbhvsn7pmjzh5wumus1e.png";
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
