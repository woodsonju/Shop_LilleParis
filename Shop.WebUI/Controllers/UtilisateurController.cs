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
    public class UtilisateurController : Controller
    {
        private IRepository<Utilisateur> utilisateurDao;
        private IUtilisateurService userService;
        private IUtilisateurRepository userCustomRepository;

        public UtilisateurController()
        {
            utilisateurDao = new SQLRepository<Utilisateur>(new MyContext());
            userCustomRepository = new UtilisateurRepository(new MyContext());
            userService = new UtilisateurService(utilisateurDao, userCustomRepository);
        }


        // GET: Utilisateur
        public ActionResult Index()
        {
            List<Utilisateur> users = userService.FindUtilisateurs();
            return View(users);
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utilisateur utilisateur = userService.FindById((int)id);
            if(utilisateur == null)
            {
                return HttpNotFound();
            }

            return View(utilisateur);
        }
    }
}