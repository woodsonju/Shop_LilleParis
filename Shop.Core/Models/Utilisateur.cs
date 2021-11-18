using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Models
{
    public class Utilisateur : BaseEntity
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string ConfirmPWD { get; set; }

        public TypeUtilisateur TypeUtilisateur { get; set; }

        public string Photos { get; set; }

        public Utilisateur()
        {
        }

        public Utilisateur(string userName, string email, string password, string confirmPWD)
        {
            UserName = userName;
            Email = email;
            Password = password;
            ConfirmPWD = confirmPWD;
        }
    }
}
