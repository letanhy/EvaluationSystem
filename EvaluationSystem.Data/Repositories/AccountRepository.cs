using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();

        public Account Login( string username, string password)
        {
            return context.Accounts.SingleOrDefault(x => x.UserName == username && x.Password == password);
        }
    }
}
