using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Entities
{
    public class Class : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? MajorsId { get; set; }
        public Majors Majors { get; set; }
        public string Note { get; set; }
    }
}
