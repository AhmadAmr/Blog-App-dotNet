using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Create(ArticleCreateViewModel createViewModel)
        {
            Console.WriteLine(createViewModel);
            return View();
        }

    }
}
