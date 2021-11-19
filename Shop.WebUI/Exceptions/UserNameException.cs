using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.WebUI.Exceptions
{
    public class UserNameException : Exception
    {
        public UserNameException()
        {
        }

        public UserNameException(string message) : base(message)
        {
        }
    }
}