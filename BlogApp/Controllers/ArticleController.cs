using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult ArticleDetail(int ? id )
        {

            var article = _context.Articles.Include(x => x.Author).FirstOrDefault(x => x.Id == id);
            return View(article);
        }


        [Authorize]
        public IActionResult Create()
        {
            
            return View(new ArticleCreateViewModel());
        }
        [Authorize]
        public IActionResult Edit(int ? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _context.Articles.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            

            return View(new ArticleCreateViewModel { article=obj});
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
            return View(nameof(Create),createViewModel);
        }
        
        [HttpPost]
        public async Task<ActionResult<Article>> EditArticle(ArticleCreateViewModel editViewModel , EditHistory editHistory)
        {
            
            var model = _context.Articles.FirstOrDefault(x => x.Id == editViewModel.article.Id);
            model.Title = editViewModel.article.Title;
            model.Content = editViewModel.article.Content;
            model.Published = editViewModel.article.Published;
            editHistory.user= await _userManager.GetUserAsync(User);
            editHistory.UpdatedOn = DateTime.Now;
            model.EditHistories = new List<EditHistory>{ editHistory} ;

            if (editViewModel.HeaderImage != null)
            {
                string ImgName = UploadedFile(editViewModel);
                model.Image = ImgName;
            }

            _context.Add(editHistory);
            _context.Update(model);
            await _context.SaveChangesAsync();
            return model;
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
