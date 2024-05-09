using Microsoft.AspNetCore.Mvc;
using MusicLove.Models;
using MusicLove.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using MusicLove.Azure;
using MusicLove.Enum;
using Microsoft.EntityFrameworkCore;
namespace MusicLove.Controllers;

public class BlogController : Controller
{
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IBlogRepository blogRepository;
    private readonly IImageRepository imageRepository;
    private readonly IAzureStorage azureStorage;
    public BlogController(IBlogRepository blogRepository, IWebHostEnvironment webHostEnvironment, IAzureStorage azureStorage, IImageRepository imageRepository)
    {
        this.blogRepository = blogRepository;
        this.webHostEnvironment = webHostEnvironment;
        this.azureStorage = azureStorage;
        this.imageRepository = imageRepository;
    }

    public IActionResult Index()
    {
        return List();
    }

    public IActionResult List()
    {
        return View("List");
    }

    public IActionResult GetNextBlogs(int skip, int take)
    {
        List<Blog> blogs = blogRepository.GetAll().Reverse().ToList();
        BlogListViewModel blogListViewModel = new BlogListViewModel();

        if (blogs.Count < skip)
        {
            blogListViewModel.Blogs = new List<Blog>();
            blogListViewModel.Last = true;
        }
        else if (blogs.Count <= skip + take)
        {
            blogListViewModel.Blogs = blogs.GetRange(skip, blogs.Count - skip);
            blogListViewModel.Last = true;
        }
        else
        {
            blogListViewModel.Blogs = blogs.GetRange(skip, take);
            blogListViewModel.Last = false;
        }

        return PartialView("ListPartial", blogListViewModel);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View("Create");
    }

    [Authorize]
    public IActionResult Edit(int id)
    {
        Blog blogModel = blogRepository.Get(blog => blog.Id == id);

        if (blogModel == null)
        {
            return Index();
        }
        else
        {
            return View("Edit", blogModel);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Upload(BlogCreateViewModel blogCreate)
    {
        if (ModelState.IsValid == false)
        {
            TempData[Define.Toastr.ERROR] = "Missing Data";
            return View("Create", blogCreate);
        }
        else if (blogRepository.Exists(blog => blog.Title == blogCreate.Title) == true)
        {
            TempData[Define.Toastr.ERROR] = "Duplicate Title";
            return View("Create", blogCreate);
        }

        Blog newBlog = new Blog()
        {
            Title = blogCreate.Title,
            Description = blogCreate.Description,
            Author = User.Identity?.Name,
            DateTime = DateTime.Now
        };

        switch (blogCreate.UploadType)
        {
            case UploadType.Image:
            {
                if (blogCreate.Files.Count == 0)
                {
                    TempData[Define.Toastr.ERROR] = "No Image Selected";
                    return View("Create", blogCreate);
                }
                
                var fileNames =  blogCreate.Files.Select(file => file.FileName);
                List<string> extensions = fileNames.Select(fileName => Path.GetExtension(fileName).ToLowerInvariant()).ToList();
                if (extensions.Exists(extension => Define.Azure.IMAGE_FILE_FORMATS.Contains(extension) == false))
                {
                    TempData[Define.Toastr.ERROR] = "Invalid Image Type";
                    return View("Create", blogCreate);
                }
                
                List<long> fileSizes = blogCreate.Files.Select(file => file.Length).ToList();
                if (fileSizes.Exists(fileSize => fileSize > Define.Azure.FILE_SIZE_LIMIT))
                {
                    TempData[Define.Toastr.ERROR] = "File size exceeds 5MB";
                    return View("Create", blogCreate);
                }
                
                blogRepository.Add(newBlog);
                blogRepository.Save();

                for (int i = 0; i < blogCreate.Files.Count; i ++)
                {
                    IFormFile formFile = blogCreate.Files[i];
                    string blobName = "Blog/" + blogCreate.Title.Replace(" ", "_") + $"_{i}.jpg";
                    await azureStorage.UploadFile(formFile, blobName);

                    string url = Define.Azure.BLOB_URL + blobName;

                    Image image = new Image()
                    {
                        Url = url,
                        BlogId = newBlog.Id,
                    };

                    imageRepository.Add(image);

                    if (i == 0)
                    {
                        Blog blog = blogRepository.Get(blog => blog.Id == newBlog.Id, isTracked: true);
                        blog.Thumbnail = url;
                    }
                }

                imageRepository.Save();

                break;
            }

            case UploadType.YouTube:
            {
                if (string.IsNullOrEmpty(blogCreate.Link) == true)
                {
                    TempData[Define.Toastr.ERROR] = "No Link Added";
                    return View("Create", blogCreate);
                }

                if (Define.Youtube.TryGetId(blogCreate.Link, out string id) == false)
                {
                    TempData[Define.Toastr.ERROR] = "Invalid Link";
                    return View("Create", blogCreate);
                }

                blogRepository.Add(newBlog);
                blogRepository.Save();

                Blog blog = blogRepository.Get(blog => blog.Id == newBlog.Id, isTracked: true);

                blog.YouTube = id;
                blog.Thumbnail = Define.Youtube.GetThumbnail(id);
                break;
            }
            default:
            {
                blogRepository.Add(newBlog);
                blogRepository.Save();

                break;
            }
        }

        TempData[Define.Toastr.SUCCESS] = "Added successfully";

        return Index();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Update(int id, string newDescription)
    {
        Blog targetBlog = blogRepository.Get(blog => blog.Id == id);

        if (targetBlog == null)
        {
            TempData[Define.Toastr.ERROR] = "Incorrect ID";
            return Index();
        }

        if (string.IsNullOrEmpty(newDescription) == true)
        {
            TempData[Define.Toastr.ERROR] = "Description Empty";
            return Edit(id);
        }

        targetBlog.Description = newDescription; // For now, only update description
        blogRepository.Update(targetBlog);
        blogRepository.Save();

        TempData[Define.Toastr.SUCCESS] = "Updated successfully";

        return Post(id);
        
    }
    
    public IActionResult Post(int id)
    {
        Blog blogModel = blogRepository.Get(filter: blog => blog.Id == id, include: query => query.Include(blog => blog.Images));

        if (blogModel == null)
        {
            return Index();
        }
        else
        {
            return View("Post", blogModel);
        }
    }

    [Authorize]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        Blog blogModel = blogRepository.Get(blog => blog.Id == id);

        if (blogModel != null)
        {
            blogRepository.Delete(blogModel);
            blogRepository.Save();

            foreach (Image image in blogModel.Images)
            {
                azureStorage.DeleteFile(image.Url.Replace(Define.Azure.BLOB_URL, string.Empty));
                imageRepository.Delete(image);
            }
            imageRepository.Save();

            TempData[Define.Toastr.SUCCESS] = "Removed Successfully!";
            return Ok();
        }
        else
        {
            TempData[Define.Toastr.ERROR] = "Remove Failed";
            return NotFound();
        }
    }
}