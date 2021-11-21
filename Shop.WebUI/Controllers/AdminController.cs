using Shop.Core.Models;
using Shop.DataAccess.SQL;
using Shop.DataAccess.SQL.LogicMetier;
using Shop.WebUI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shop.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IRepository<Utilisateur> utilisateurDao;
        private IUtilisateurService userService;
        private IUtilisateurRepository userCustomRepository;

        public AdminController()
        {
            utilisateurDao = new SQLRepository<Utilisateur>(new MyContext());
            userService = new UtilisateurService(utilisateurDao, userCustomRepository);
        }

        // GET: Admin
        public ActionResult Index()
        {
            List<Utilisateur> users = userService.FindUtilisateurs();
            return View(users);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Utilisateur user = userService.FindById((int)id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(user);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Utilisateur user, int id)
        {
            if (ModelState.IsValid)
            {
                Utilisateur uBDD = userService.FindById(id);
                if (uBDD == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    uBDD.TypeUtilisateur = user.TypeUtilisateur;
                    userService.Update(uBDD);
                    userService.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Utilisateur user = userService.FindById((int)id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(user);
                }
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Utilisateur user, int id)
        {
            userService.DeleteById(id);
            userService.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}