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
    public class ClassRepository : IClassRepository
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();
        public int Add(Class model)
        {
            context.Classes.Add(model);
            context.SaveChanges();
            return model.Id;
        }
        public void Update(Class model)
        {
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(int Id)
        {
            var model = GetById(Id);
            context.Classes.Remove(model);
            context.SaveChanges();
        }
        public void Delete(Class _class)
        {
            context.Classes.Remove(_class);
            context.SaveChanges();
        }
        public void DeleteRs(int Id)
        {
            var model = context.Classes.SingleOrDefault(x => x.Id == Id);
            model.IsDeleted = true;
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public IQueryable<Class> ListAll()
        {
            return context.Classes.Include(x => x.Majors.Faculty).Where(x => x.IsDeleted != true);
        }
        public IQueryable<Class> ListAllInfo()
        {
            return context.Classes.Include(x=>x.Majors).Where(x => x.IsDeleted != true);
        }
        public IQueryable<Class> GetClassByMajors(int majorsId)
        {
            return context.Classes.Include(x => x.Majors).Where(x => x.IsDeleted != true && x.MajorsId == majorsId);
        }
        public Class GetById(int Id)
        {
            var model = context.Classes.SingleOrDefault(x => x.Id == Id);
            return model;
        }
        public Class GetInfoById(int Id)
        {
            var model = context.Classes.Include(x=>x.Majors.Faculty).SingleOrDefault(x => x.Id == Id);
            return model;
        }

        public int GetCount()
        {
            return context.Classes.Count(x => x.IsDeleted != true);
        }
    }
}
