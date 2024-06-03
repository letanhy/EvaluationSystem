using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Entities
{
    public class ContentEvaluation : BaseEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
