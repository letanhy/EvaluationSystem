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
    public class FacultyRepository : IFacultyRepository 
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();
        public int Add(Faculty model)
        {
            context.Faculty.Add(model);
            context.SaveChanges();
            return model.Id;
        }
        public void Update(Faculty model)
        {
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            context.Faculty.Remove(model);
            context.SaveChanges();
        }
        public void Delete(Faculty faculty)
        {
            context.Faculty.Remove(faculty);
            context.SaveChanges();
        }
        public void DeleteRs(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            model.IsDeleted = true;
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public IEnumerable<Faculty> ListAll()
        {
            return context.Faculty;
        }
        public IEnumerable<Faculty> ListAllInfo()
        {
            return context.Faculty.Where(x => x.IsDeleted != true);
        }
        
        public Faculty GetById(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            return model;
        }
        public Faculty GetInfoById(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            return model;
        }
    }
}
