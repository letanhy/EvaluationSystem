using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAll();
        IQueryable<Category> GetAllBySemester();
        IQueryable<Category> GetAllByCourse();
        IQueryable<Category> GetAllInfo();
        Category GetById(int Id);
        Category GetInfoById(int Id);
        int Add(Category category);
        void Update(Category category);
        void Delete(int Id);
        void DeleteRs(int Id);
    }
}
