using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace EvaluationSystem.Models
{
    public class Principal : IPrincipal
    {
        public IIdentity Identity { get; set; }
        public bool IsInRole(string role)
        {
            return true;
        }
        public Principal()
        {

        }
        public Principal(string userName)
        {
            Identity = new GenericIdentity(userName);
        }

        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
