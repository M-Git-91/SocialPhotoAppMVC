using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using SocialPhotoAppMVC.Enums;

namespace SocialPhotoAppMVC.Models
{
    public class Photo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public Category Category { get; set; }
        public bool IsFeatured { get; set; } = false;
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public AppUser User { get; set; }
        public List<Album> Albums { get; set; }
    }
}
