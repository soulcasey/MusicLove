using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicLove.Data;
using MusicLove.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MusicLove.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using MusicLove.Azure;
using System;
namespace MusicLove.Controllers;

public class BlogController : Controller
{
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IBlogRepository blogRepository;
    private readonly IAzureStorage azureStorage;
    public BlogController(IBlogRepository blogRepository, IWebHostEnvironment webHostEnvironment, IAzureStorage azureStorage)
    {
        this.blogRepository = blogRepository;
        this.webHostEnvironment = webHostEnvironment;
        this.azureStorage = azureStorage;
    }

    public IActionResult Index()
    {
        return List();
    }

    public IActionResult List()
    {
        BlogListViewModel blogListViewModel = new()
        {
            Blogs = blogRepository.GetAll().Reverse().ToList()
        };

        return View("List", blogListViewModel);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View("Create");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(Blog newBlog, IFormFile? file)
    {
        if (ModelState.IsValid == false)
        {
            TempData[Define.Toastr.ERROR] = "Error";
            return View("Create", newBlog);
        }
        else if (blogRepository.Exists(blog => blog.Title == newBlog.Title) == true)
        {
            TempData[Define.Toastr.ERROR] = "Duplicate Title";
            return View("Create", newBlog);
        }
        else
        {
            if (file != null)
            {
                string address = newBlog.Title.Replace(" ", "_");
                string blobName = "Blog/" + address + ".jpg";
                await azureStorage.UploadFile(file, blobName);
                newBlog.Image = Define.Azure.BLOB_URL + blobName;
            }

            newBlog.Author = User.Identity.Name;

            blogRepository.Add(newBlog);
            blogRepository.Save();

            TempData[Define.Toastr.SUCCESS] = "Added successfully";

            return RedirectToAction("Index");
        }
    }
    
    public IActionResult Post(int id)
    {
        Blog blogModel = blogRepository.Get(blog => blog.Id == id);

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
    public IActionResult Edit()
    {
        return View("Create");
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
            azureStorage.DeleteFile(blogModel.Image.Replace(Define.Azure.BLOB_URL, string.Empty));

            TempData[Define.Toastr.SUCCESS] = "Removed Successfully!";
        }
        else
        {
            TempData[Define.Toastr.ERROR] = "Remove Failed";
        }

        return Ok();
    }
}