using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(ApplicationDbContext conext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            _context = conext;
        }
        public IActionResult Create(ArticleCreateViewModel createViewModel)
        {
            
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Article>> CreateArticle (ArticleCreateViewModel createViewModel )
        {

            createViewModel.article.CreatedOn = DateTime.Now;
            createViewModel.article.Author = await _userManager.GetUserAsync(User);
            _context.Add(createViewModel.article);
            _context.SaveChanges();

            return createViewModel.article;
        }

    }
}
