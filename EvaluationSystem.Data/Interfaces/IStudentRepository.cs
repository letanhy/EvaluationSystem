using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface IStudentRepository
    {
        IQueryable<Student> ListAll();
        Student GetById(int Id);
        Student GetInfoById(int Id);
        int Add(Student student);
        void Update(Student student);
        void Delete(int Id);
        void DeleteRs(int Id);
        IEnumerable<Student> ListAllInfo();
    }
}
