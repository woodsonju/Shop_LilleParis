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
    public class CategoryController : Controller
    {
        IRepository<Category> categoryDao;

        public CategoryController()
        {
            categoryDao = new SQLRepository<Category>(new MyContext());
        }

        // GET: Category
        public ActionResult Index()
        {
            List<Category> categories = categoryDao.Collection().ToList();
            return View(categories);
        }


        public ActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                categoryDao.Insert(category);
                categoryDao.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            Category category = categoryDao.FindById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category, int id)
        {
            //Avec l'Id, on recupère la categorie dans la base de données
            Category catToEdit = categoryDao.FindById(id);

            //On verifie qu'elle existe bien dans la base de données
            if (catToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }
                else
                {
                    //On met à jour la categorie de la base de données avec la categorie venant du formulaire
                    catToEdit.CategoryName = category.CategoryName;
                    categoryDao.Update(catToEdit);
                    categoryDao.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(int id)
        {
            Category category = categoryDao.FindById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Category catToDelete = categoryDao.FindById(id);

            if(catToDelete == null)
            {
                return HttpNotFound();
            } else
            {
                categoryDao.DeleteById(id);
                categoryDao.SaveChanges();
                return RedirectToAction("Index");
            }

        }

    }
}