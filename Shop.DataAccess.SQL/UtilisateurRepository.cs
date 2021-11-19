using Shop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.SQL
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private MyContext DataContext;

        public UtilisateurRepository(MyContext dataContext)
        {
            DataContext = dataContext;
        }

        public Utilisateur FindByMail(string email)
        {
            //1- Syntaxe LINQ : Convient aux dev sql
            /*       Utilisateur u1 = (from u in DataContext.Utilisateurs
                                     where u.Email.Contains(email)
                                     select u).SingleOrDefault();*/

            //2- Chainage de méthode : Convient aux devs (utilisation d'expression lambda)
            return DataContext.Utilisateurs.SingleOrDefault(u => u.Email == email);
        }
    }
}
