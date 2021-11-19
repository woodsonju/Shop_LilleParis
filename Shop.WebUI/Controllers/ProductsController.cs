using Shop.Core.Models;
using Shop.DataAccess.SQL;
using Shop.DataAccess.SQL.LogicMetier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        IRepository<Product> productDao;
        IRepository<Category> categoryDao;


        public ProductsController()
        {
            productDao = new SQLRepository<Product>(new MyContext());
            categoryDao = new SQLRepository<Category>(new MyContext());
        }

        // GET: Products
        public ActionResult Index()
        {
            List<Product> products = productDao.Collection().ToList();
            return View(products);
        }
    }
}