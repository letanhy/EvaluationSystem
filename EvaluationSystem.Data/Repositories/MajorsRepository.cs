using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;

namespace EvaluationSystem.Data.Repositories
{
    public class MajorsRepository : IMajorsRepository
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();

        public int Add(Majors majors)
        {
            context.Majors.Add(majors);
            context.SaveChanges();

            return majors.Id;
        }
        public void Update(Majors model)
        {
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(int Id)
        {
            var model = context.Majors.SingleOrDefault(x => x.Id == Id);
            context.Majors.Remove(model);
            context.SaveChanges();
        }
        public void DeleteRs(int Id)
        {
            var model = context.Majors.SingleOrDefault(x => x.Id == Id);
            model.IsDeleted = true;
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public IEnumerable<Majors> ListAll()
        {
            return context.Majors.Where(x => x.IsDeleted != true).ToList();
        }
        public Majors GetById(int Id)
        {
            return context.Majors.SingleOrDefault(x => x.Id == Id);
        }

        public IEnumerable<Majors> ListAllInfo()
        {
            return context.Majors.Include(x=>x.Faculty).Where(x => x.IsDeleted != true);
        }

        public Majors GetInfoById(int Id)
        {
            return context.Majors.Include(x => x.Faculty).SingleOrDefault(x => x.Id == Id);
        }

        public void Delete(Majors majors)
        {
            context.Majors.Remove(majors);
            context.SaveChanges();
        }
    }
}
