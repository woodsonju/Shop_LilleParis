using Shop.Core.Models;
using Shop.Core.Tools;
using Shop.DataAccess.SQL;
using Shop.DataAccess.SQL.LogicMetier;
using Shop.WebUI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.WebUI.Service
{
    public class UtilisateurService : IUtilisateurService
    {
        private IRepository<Utilisateur> userDao;
        private IUtilisateurRepository userDCustomRepository;

        public UtilisateurService()
        {
    
        }

        public UtilisateurService(IRepository<Utilisateur> userDao, IUtilisateurRepository userDCustomRepository)
        {
            this.userDao = userDao;
            this.userDCustomRepository = userDCustomRepository;
        }



        /// <summary>
        /// Il faut que l'utilisateur existe dans la base de données et que son mot de passe soit le même que celui
        /// dans la base de données
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Utilisateur CheckLogin(string email, string password)
        {
            Utilisateur u = userDCustomRepository.FindByMail(email);
            string pwdCrypt = password;

            //Haché le mot de passe entré par l'utilisateur
            pwdCrypt = HashTools.ComputeSha256Hash(password);

            if(u == null || !u.Password.Equals(pwdCrypt))
            {
                return null;
            }
            return u;
        }

        public void DeleteById(int id)
        {
            userDao.DeleteById(id);
        }

        public Utilisateur FindById(int id)
        {
            return userDao.FindById(id);
        }

        public Utilisateur FindByMail(string mail)
        {
            return userDCustomRepository.FindByMail(mail);
        }

        public List<Utilisateur> FindUtilisateurs()
        {
            return userDao.Collection().ToList();
        }

        public void InsertUser(Utilisateur u)
        {
            //On va verifier que cet utilisateur n'existe pas dans la base de données
            VerifyMailAndUserName(u);

            //Avant d'inserer l'utilisateur 
            //On met à our le password de l'utilisateur avec le password crypté
            u.Password = HashTools.ComputeSha256Hash(u.Password);
            u.ConfirmPWD = HashTools.ComputeSha256Hash(u.Password);

            userDao.Insert(u);
        }

   
        public void SaveChanges()
        {
            userDao.SaveChanges();
        }


        public void Update(Utilisateur u)
        {
            Utilisateur userBDD = userDao.FindById(u.Id);

            //On crypte le  password et le confirmPWD avant de l'enregistrer dans la base de données
            if(!userBDD.Password.Equals(u.Password))
            {
                u.Password = HashTools.ComputeSha256Hash(u.Password);
                u.ConfirmPWD = HashTools.ComputeSha256Hash(u.ConfirmPWD);
            }

            userDao.Update(u);
        }


        /// <summary>
        /// Dans la liste des utilisateurs qui sont dans la base de données on va chercher si cette utilisateur 
        /// à un UserName et un email existant
        /// </summary>
        /// <param name="u"></param>
        private void VerifyMailAndUserName(Utilisateur u)
        {
            List<Utilisateur> userFromBDD = FindUtilisateurs();
            for (int i = 0; i < userFromBDD.Count; i++)
            {
                if(u.UserName == userFromBDD[i].UserName)
                {
                    throw new UserNameException("Le UserName existe déjà. Veuillez choisir un autre Username!!");
                }

                if (u.Email == userFromBDD[i].Email)
                {
                    throw new EmailException("L'adresse mail existe déjà. Veuillez choisir un autre adresse mail!!");
                }
            }
        }

    }
}