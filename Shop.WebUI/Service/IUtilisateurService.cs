using Shop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.WebUI.Service
{
    public interface IUtilisateurService
    {
        Utilisateur CheckLogin(string mail, string password);
        void InsertUser(Utilisateur u);
        void SaveChanges();
        Utilisateur FindByMail(string mail);
        List<Utilisateur> FindUtilisateurs();
        Utilisateur FindById(int id);
        void DeleteById(int id);
        void Update(Utilisateur u);
    }
}
