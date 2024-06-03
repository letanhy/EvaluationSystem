using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface IFacultyRepository
    {
        IEnumerable<Faculty> ListAll();
        Faculty GetById(int Id);
        Faculty GetInfoById(int Id);
        int Add(Faculty faculty);
        void Update(Faculty faculty);
        void Delete(int Id);
        void DeleteRs(int Id);
        IEnumerable<Faculty> ListAllInfo();
    }
}
