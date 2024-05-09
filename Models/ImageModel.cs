using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicLove.Models;

public class Image
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Url { get; set; }
    
    [ForeignKey(nameof(Blog))]
    public int BlogId { get; set; }

    [ForeignKey(nameof(BlogId))]
    public Blog Blog { get; set; }
}