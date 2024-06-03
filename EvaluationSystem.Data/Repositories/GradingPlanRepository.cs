using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Repositories
{
    internal class GradingPlanRepository : IGradingPlanRepository
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();
        public int Add(GradingPlan gradingPlan)
        {
            context.GradingPlan.Add(gradingPlan);
            context.SaveChanges();

            return gradingPlan.Id;
        }

        public void Delete(int Id)
        {
            var gradingPlan = context.GradingPlan.SingleOrDefault(x => x.Id == Id);
            context.GradingPlan.Remove(gradingPlan);
            context.Entry(gradingPlan).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void DeleteRs(int Id)
        {
            var gradingPlan = context.GradingPlan.SingleOrDefault(x => x.Id == Id);
            gradingPlan.IsDeleted = true;
            context.Entry(gradingPlan).State = EntityState.Modified;
            context.SaveChanges();
        }

        public GradingPlan GetById(int Id)
        {
            return context.GradingPlan.SingleOrDefault(x => x.Id == Id);
        }

        public GradingPlan GetInfoById(int Id)
        {
            return context.GradingPlan.SingleOrDefault(x => x.Id == Id);
        }

        public IEnumerable<GradingPlan> ListAll()
        {
            return context.GradingPlan.Where(x => x.IsDeleted != true).ToList();
        }

        public IEnumerable<GradingPlan> ListAllInfo()
        {
            return context.GradingPlan.Where(x => x.IsDeleted != true).ToList();
        }

        public void Update(GradingPlan gradingPlan)
        {
            context.Entry(gradingPlan).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
