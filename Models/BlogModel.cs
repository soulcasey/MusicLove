using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicLove.Models;

public class Blog
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    [DisplayName("Image")]
    public string? Image { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public string Author { get; set; } = "Casey"; // Temporary

    [Required, DataType(DataType.DateTime)]
    public DateTime DateTime { get; set; } = DateTime.Now;
}