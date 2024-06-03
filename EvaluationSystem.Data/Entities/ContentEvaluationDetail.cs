using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Entities
{
    public class ContentEvaluationDetail
    {
        public int Id { get; set; }
        public string Cointent { get; set; }
        public int? CriteriaId { get; set; }
        public int? ContentEvaluationId { get; set; }
        public int? CriteriaCategoryId { get; set; }

    }
}
