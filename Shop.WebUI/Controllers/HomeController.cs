using Shop.Core.Models;
using Shop.Core.ViewModels;
using Shop.DataAccess.SQL;
using Shop.DataAccess.SQL.LogicMetier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> productDao;
        IRepository<Category> categoryDao;


        public HomeController()
        {
            productDao = new SQLRepository<Product>(new MyContext());
            categoryDao = new SQLRepository<Category>(new MyContext());
        }

        public ActionResult Index(string Category = null)
        {
            List<Product> products;
            List<Category> categories = categoryDao.Collection().ToList();

            //Si Category est égal à null (Si on ne selectionne pas une catégorie), on retourne la liste des Produits
            //Sinon on retourne les produits de la categorie mentionnée (selectionnée)
            if(Category == null)
            {
                products = productDao.Collection().ToList();
            } else
            {
                products = productDao.Collection().Where(p => p.Category == Category).ToList();
            }

            ProductListViewModel viewModel = new ProductListViewModel();

            viewModel.Products = products;
            viewModel.Categories = categories;

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}