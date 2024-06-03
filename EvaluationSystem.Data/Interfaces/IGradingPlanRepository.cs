using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface IGradingPlanRepository
    {
        IEnumerable<GradingPlan> ListAll();
        GradingPlan GetById(int Id);
        GradingPlan GetInfoById(int Id);
        int Add(GradingPlan gradingPlan);
        void Update(GradingPlan gradingPlan);
        void Delete(int Id);
        void DeleteRs(int Id);
        IEnumerable<GradingPlan> ListAllInfo();
    }
}
