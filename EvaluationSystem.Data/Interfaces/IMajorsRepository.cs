using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface IMajorsRepository
    {
        IEnumerable<Majors> ListAll();
        Majors GetById(int Id);
        int Add(Majors majors);
        void Update(Majors majors);
        void Delete(int Id);
        void DeleteRs(int Id);
        void Delete(Majors majors);
        IEnumerable<Majors> ListAllInfo();
        Majors GetInfoById(int Id);
    }
}
