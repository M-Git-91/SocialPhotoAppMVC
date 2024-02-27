using SocialPhotoAppMVC.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace SocialPhotoAppMVC.ViewModels
{
    public class PhotoDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public Category Category { get; set; }
        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public AppUser User { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();
        public IPagedList<Comment>? Comments { get; set; }
    }
}
