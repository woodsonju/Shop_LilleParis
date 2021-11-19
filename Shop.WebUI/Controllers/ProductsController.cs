using Shop.Core.Models;
using Shop.Core.ViewModels;
using Shop.DataAccess.SQL;
using Shop.DataAccess.SQL.LogicMetier;
using System;
using System.Collections.Generic;
using System.IO;
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


        public ActionResult Create()
        {
            ProductCategoryViewModel viewModel = new ProductCategoryViewModel();
            viewModel.Product = new Product();
            viewModel.Categories = categoryDao.Collection();
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase image)
        {
            if(!ModelState.IsValid)
            {
                return View(product);
            } else
            {
                if(image != null)
                {
                    int maxId;
                    try
                    {
                        maxId = productDao.Collection().Max(p => p.Id);
                    }
                    catch (Exception)
                    {

                        maxId = 0;
                    }

                    int nextId = maxId + 1;

                    //On va sauvegarder notre image dans un dossier prodImages, qui se trouve dans un dossier Content
                    product.Image = nextId + Path.GetExtension(image.FileName);
                    image.SaveAs(Server.MapPath("~/Content/prodImages/") + product.Image);  
                }

                Session["Image"] = product.Image;

                productDao.Insert(product);
                productDao.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            //On récupere le produit à modifier
            Product p = productDao.FindById(id);
            if(p == null)
            {
                return HttpNotFound();
            }else
            {
                ProductCategoryViewModel viewModel = new ProductCategoryViewModel();

                //On met à jour Produit de viewModel via le produit récuperer(à modifier)
                viewModel.Product = p;
                Session["Image"] = p.Image;

                viewModel.Categories = categoryDao.Collection();

                return View(viewModel);
            }
        }
      

     }
}