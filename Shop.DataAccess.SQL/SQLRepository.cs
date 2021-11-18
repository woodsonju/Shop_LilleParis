using Shop.Core.Models;
using Shop.DataAccess.SQL.LogicMetier;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.SQL
{
    /// <summary>
    /// Classe générique qu'on pourra utiliser pour n'importe quelle entité
    /// Cette classe va contenir toutes les méthodes classiques permettant de gerer les entités (classes)
    /// Remarque: Pour les méthodes spécifique  à une classe on créera une "ClasseRepository" par exemple UtilisateurRepository
    /// Méthodes classiques : FindById - List - DeleteById - Add/Insert  - Update 
    /// Methode specifique à une classe : FindByMail, FindByKey
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        //Regroupe l'ensemble des entité gérées
        internal MyContext DataContext;

        internal DbSet<T> dbSet;

        public SQLRepository(MyContext dataContext)
        {
            DataContext = dataContext;

            //Crée un DbSet pour l'entité T : On va recuperer le dbSet de l'entité T grâce au contexte
            dbSet = DataContext.Set<T>();
        }

        
        public IQueryable<T> Collection()
        {
            return dbSet;
        }


        public T FindById(int id)
        {
            //1er façon 
            //return dbSet.Find(id);

            //2ème façon
            return dbSet.AsNoTracking().SingleOrDefault(x => x.Id == id);
        }

        public void DeleteById(int id)
        {
            T t = FindById(id);

            if(DataContext.Entry(t).State == EntityState.Detached)
            {
                //Attachee l'entité donnée au contexte
                dbSet.Attach(t);
            }

            dbSet.Remove(t);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void SaveChanges()
        {
            DataContext.SaveChanges();
        }

        public void Update(T t)
        {
            dbSet.Attach(t); //Charge l'objet t dans le context
            DataContext.Entry(t).State = EntityState.Modified;
        }
    }
}
