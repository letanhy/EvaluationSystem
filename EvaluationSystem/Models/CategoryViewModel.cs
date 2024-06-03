using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Mã")]
        [Required(ErrorMessage = "Vui lòng điền mã")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên")]
        [Display(Name = "Tên")]
        public string Name { get; set; }
        public string Group { get; set; }
        public int? OrderNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}