using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArticleController(ApplicationDbContext conext, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            webHostEnvironment = hostEnvironment;
            _context = conext;
        }
        [Authorize]
        public IActionResult Create(ArticleCreateViewModel createViewModel)
        {
            
            return View();
        }
        public IActionResult Edit(int ? id, ArticleCreateViewModel createViewModel)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _context.Articles.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            createViewModel.article = obj;

            return View(createViewModel);
        }

        [HttpPost]
        
        public async Task<ActionResult<Article>> CreateArticle (ArticleCreateViewModel createViewModel )
        {
            if (ModelState.IsValid)
            {
                string ImgName = UploadedFile(createViewModel);
                createViewModel.article.CreatedOn = DateTime.Now;
                createViewModel.article.Image = ImgName;
                createViewModel.article.Author = await _userManager.GetUserAsync(User);
                _context.Add(createViewModel.article);
                await _context.SaveChangesAsync();
                return createViewModel.article;

            }
            return View(nameof(Index),createViewModel);
        }

        private string UploadedFile(ArticleCreateViewModel model)
        {
            string ImgName = null;

            if (model.HeaderImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                ImgName = Guid.NewGuid().ToString() + "_" + model.HeaderImage.FileName;
                string filePath = Path.Combine(uploadsFolder, ImgName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.HeaderImage.CopyTo(fileStream);
                }
            }
            return ImgName;
        }

    }
}
