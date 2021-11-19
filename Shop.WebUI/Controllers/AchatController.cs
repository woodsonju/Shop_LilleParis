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
    public class AchatController : Controller
    {

        IRepository<Product> productDao;

        //Represente la liste des produits dans le panier
        List<Product> lstProd = new List<Product>();

        //Le prix total des produits dans le panier
        decimal total = 0;

        public AchatController()
        {
            productDao = new SQLRepository<Product>(new MyContext());
        }



        /// <summary>
        /// Permet d'ajouter un produit dans un panier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Ajouter(int id)
        {
            Product p = productDao.FindById(id);

            if (Session["Products"] == null)
            {
                lstProd.Add(p);
                Session["Products"] = lstProd;
                Session["nbProd"] = 1;
                Session["total"] = p.Price;
            } else
            {
                lstProd = (List<Product>)Session["Products"];
                lstProd.Add(p);
                Session["Products"] = lstProd;

                //TOtal
                foreach (var item in lstProd)
                {
                    total += item.Price;
                }

                Session["total"] = total;
                Session["nbProd"] = lstProd.Count;
            }

            return RedirectToAction("Index", "home");
        }


        /// <summary>
        /// On retoune la liste des produits venant de la session["Products"] vers la vue Panier.cshtml
        /// </summary>
        /// <returns></returns>
        public ActionResult Panier()
        {
            lstProd = (List<Product>)Session["Products"];
            return View(lstProd);
        }
    }
}