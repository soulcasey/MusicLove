using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MusicLove.Enum;

namespace MusicLove.Models;

public class BlogCreateViewModel
{
    [Required]
    [DisplayName("Title")]
    [MaxLength(20)]
    public required string Title { get; set; }

    public UploadType UploadType { get; set; } = UploadType.None;
    
    public string Link { get; set; }
    
    public List<IFormFile> Files { get; set; } = new List<IFormFile>(); 

    [Required]
    public required string Description { get; set; }
}   