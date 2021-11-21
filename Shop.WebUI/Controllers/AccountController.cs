using Shop.Core.Models;
using Shop.DataAccess.SQL;
using Shop.DataAccess.SQL.LogicMetier;
using Shop.WebUI.Exceptions;
using Shop.WebUI.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shop.WebUI.Controllers
{
    /// <summary>
    /// Permettre à l'utilisateur de se connecter, de se deconnecter, de modifier son compte
    /// </summary>
    public class AccountController : Controller
    {
        private IRepository<Utilisateur> utilisateurDao;
        private IUtilisateurService userService;
        private IUtilisateurRepository userCustomRepository;

        public AccountController()
        {
            utilisateurDao = new SQLRepository<Utilisateur>(new MyContext());
            userCustomRepository = new UtilisateurRepository(new MyContext());
            userService = new UtilisateurService(utilisateurDao, userCustomRepository);
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Permettre à l'utilisateur de créer un compte
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Utilisateur u)
        {
            if (ModelState.IsValid)
            {
                if (u.Password.Equals(u.ConfirmPWD))
                {
                    Utilisateur user = new Utilisateur(u.UserName, u.Email, u.Password, u.ConfirmPWD);
                    user.Photos = "imgProfile.png";
                    try
                    {
                        userService.InsertUser(user);  //Ajoute l'entité u au contexte
                    }
                    catch (UserNameException eUserName)
                    {
                        ViewBag.ExceptionUserName = eUserName.Message;
                        return View();
                    }
                    catch (EmailException eUserName)
                    {
                        ViewBag.EmailException = eUserName.Message;
                        return View();
                    }

                    userService.SaveChanges();  //Ensuite l'ajoute dans la base de données

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.ErrorLog = "Les deux mots de passe ne correspondant pas";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(String email, string password)
        {
            if (ModelState.IsValid)
            {
                Utilisateur u = userService.CheckLogin(email, password);
                if (u == null)
                {
                    ModelState.Clear();
                    ViewBag.ErrorLog = "L'adresse renseignée n'existe pas ou le mot de passe est faux";
                    return View();
                }
                else
                {
                    //Connexion a reussi
                    Session["Connexion"] = u.UserName;
                    Session["TypeUtilisateur"] = u.TypeUtilisateur;
                    Session["Email"] = u.Email;
                    Session["Id"] = u.Id;
                    Session["Photo"] = u.Photos;

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.ErrorLog = "Le pseudo ou le mot de passe ne sont pas renseignés";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session["Connexion"] = null;
            Session["Id"] = null;
            Session["Photo"] = null;

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utilisateur utilisateur = userService.FindById((int)id);

            if (utilisateur == null)
            {
                return HttpNotFound();
            }

            return View(utilisateur);

        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = userService.FindById((int)id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }

            return View(utilisateur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Utilisateur user, HttpPostedFileBase photo)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                //Si on modifie la photo   ==>  photo  non null
                if (photo != null)
                {
                    user.Photos = user.UserName + Path.GetExtension(photo.FileName);
                    //Sauvegarde la photo dans le dossier AccountImages
                    photo.SaveAs(Server.MapPath("~/Content/accountImages/") + user.Photos);
                    //Actualise la session utilisateur avec la nouvelle photo
                    Session["Photo"] = user.Photos;
                }
                else
                {
                    //Dans le cas où l'ont modifie pas la photo
                    //On récupère la photo de la session avec laquelle on s'est connectée
                    user.Photos = (string)Session["Photo"];
                }

                userService.Update(user);
                userService.SaveChanges();

                return RedirectToAction("Details", new { Id = user.Id });
                //return RedirectToAction("Index");
            }


        }
    }
}