using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace SocialPhotoAppMVC.Models
{
    public class Album
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public AppUser User { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
