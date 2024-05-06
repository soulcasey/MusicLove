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
    
    [DisplayName("Image")]
    public string Image { get; set; } = string.Empty;

    [DisplayName("YouTube")]
    public string YouTube { get; set; } = string.Empty;

    [Required]
    public required string Description { get; set; }

    public string? Author { get; set; } // Temporary

    [Required, DataType(DataType.DateTime)]
    public required DateTime DateTime { get; set; }
}