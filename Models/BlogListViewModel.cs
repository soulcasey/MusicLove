using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicLove.Models;

public class BlogListViewModel
{
    public List<Blog> Blogs { get; set; }
    public bool Last { get; set; }
}