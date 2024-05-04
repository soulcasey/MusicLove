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
    public async Task<IActionResult> Add(Blog newBlog, IFormFile? file)
    {
        if (ModelState.IsValid == false)
        {
            TempData[Define.Toastr.ERROR] = "Error";
            return View("Create", newBlog);
        }
        else if (blogRepository.Exists(blog => blog.Title == newBlog.Title && blog.Id != newBlog.Id) == true)
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

            return Index();
        }
    }

    [Authorize]
    [HttpPost]
    public IActionResult Update(Blog newBlog)
    {
        Blog targetBlog = blogRepository.Get(blog => blog.Id == newBlog.Id);

        if (targetBlog == null)
        {
            TempData[Define.Toastr.ERROR] = "Error";
            return View("Edit", newBlog);
        }

        targetBlog.Description = newBlog.Description; // For now, only update description
        blogRepository.Update(targetBlog);
        blogRepository.Save();

        TempData[Define.Toastr.SUCCESS] = "Updated successfully";

        return Index();
        
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
            return Ok();
        }
        else
        {
            TempData[Define.Toastr.ERROR] = "Remove Failed";
            return NotFound();
        }
    }
}