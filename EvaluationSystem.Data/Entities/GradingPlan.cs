using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Entities
{
    public class GradingPlan : BaseEntity 
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Note { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
