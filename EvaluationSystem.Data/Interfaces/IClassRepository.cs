using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface IClassRepository 
    {
        IEnumerable<Class> ListAll();
        IEnumerable<Class> ListAllInfo();
        IEnumerable<Class> ListAllByMajors(int ClassId);
        Class GetById(int Id);
        Class GetInfoById(int Id);
        int Add(Class _class);
        void Update(Class _class);
        void Delete(int Id);
        void DeleteRs(int Id);
        void Delete(Class _class);
        int GetCount();
    }
}
