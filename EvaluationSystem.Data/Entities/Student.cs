using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Entities
{
    public class Student :BaseEntity 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? Age { get; set; }
        public string Code { get; set; }
        public int? ClassId { get; set; }
        public Class Class { get; set; }
    }
}
