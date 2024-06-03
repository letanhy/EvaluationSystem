using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluationSystem.Data.Interfaces;
namespace EvaluationSystem.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();
        public int Add(Category model)
        {
            context.Category.Add(model);
            context.SaveChanges();
            return model.Id;
        }
        public void Update(Category model)
        {
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(int Id)
        {
            var model = GetById(Id);
            context.Category.Remove(model);
            context.SaveChanges();
        }
        public void Delete(Category category)
        {
            context.Category.Remove(category);
            context.SaveChanges();
        }
        public void DeleteRs(int Id)
        {
            var model = context.Category.SingleOrDefault(x => x.Id == Id);
            model.IsDeleted = true;
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public IEnumerable<Category> ListAll()
        {
            return context.Category;
        }
        public IEnumerable<Category> ListAllInfo()
        {
            return context.Category.Where(x => x.IsDeleted != true);
        }

        public Category GetById(int Id)
        {
            var model = context.Category.SingleOrDefault(x => x.Id == Id);
            return model;
        }

        public Category GetInfoById(int Id)
        {
            var model = context.Category.SingleOrDefault(x => x.Id == Id);
            return model;
        }

        public IEnumerable<Category> ListAllBySemester()
        {
            return context.Category.Where(x => x.IsDeleted != true && x.Group == "Hocky");
        }

        public IEnumerable<Category> ListAllByCourse()
        {
            return context.Category.Where(x => x.IsDeleted != true && x.Group == "Khoahoc");
        }
    }
}
