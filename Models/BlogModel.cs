using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicLove.Models;

public class Blog
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public required string Title { get; set; }

    public string Thumbnail { get; set; } = string.Empty;
    
    [DisplayName("Images")]
    public ICollection<Image> Images { get; set; } = new List<Image>();

    [DisplayName("YouTube")]
    public string YouTube { get; set; } = string.Empty;

    [Required]
    public required string Description { get; set; }

    public string? Author { get; set; } // Temporary

    [Required, DataType(DataType.DateTime)]
    public required DateTime DateTime { get; set; }
}