using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductCategories.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("Products")]
        public IActionResult Products()
        {
            ViewBag.allProducts = _context.Products.ToList();
            return View();
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product nProd)
        {
            if(ModelState.IsValid)
            {
                _context.Add(nProd);
                _context.SaveChanges();
                return RedirectToAction("Products");
            }
            else
            {
                ViewBag.allProducts = _context.Products.ToList();
                return View("Products");
            }
        }

        [HttpGet("Products/{pId}")]
        public IActionResult ViewProduct(int pId)
        {
            

            ViewBag.AllCategories = _context.Categories.ToList();
            Product oneproduct = _context.Products
            .Include(f => f.ProdCategories)
            .ThenInclude(g => g.Category)
            .SingleOrDefault(p => p.ProductId == pId);
            
            return View(oneproduct);
        }

        [HttpPost("AddCategorytoProduct")]
        public IActionResult AddCategorytoProduct(Association newAssoc)
        {
            _context.Add(newAssoc);
            _context.SaveChanges();
            return Redirect($"/Products/{newAssoc.ProductId}");
            // return RedirectToAction ("ViewProduct", new {pId = newAssoc.ProductId});
            
        }

        [HttpGet("Categories")]
        public IActionResult Categories()
        {
            ViewBag.AllCategories = _context.Categories.ToList();
            return View();
        }

        [HttpGet("Categories/{cId}")]
        public IActionResult ViewCategory(int cId)
        {
            ViewBag.AllProducts = _context.Products.ToList();
            Category onecategory = _context.Categories
            .Include(f => f.CatProducts)
            .ThenInclude(g => g.Category)
            .SingleOrDefault(p => p.CategoryId == cId);
            
            return View(onecategory);
        }

        [HttpPost("AddProducttoCategory")]
        public IActionResult AddProducttoCategory(Association newAssoc)
        {
            _context.Add(newAssoc);
            _context.SaveChanges();
            return Redirect($"/Categories/{newAssoc.CategoryId}");
            // return RedirectToAction ("ViewProduct", new {pId = newAssoc.ProductId});
            
        }

        [HttpPost("AddCategory")]
        public IActionResult AddCategory(Category nCate)
        {
            if(ModelState.IsValid)
            {
                _context.Add(nCate);
                _context.SaveChanges();
                return RedirectToAction("Categories");
            }
            else
            {
                ViewBag.AllCategories = _context.Categories.ToList();
                return View("Categories");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
