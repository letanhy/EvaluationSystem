using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Models
{
    public class ClassViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Chưa điền tên lớp")]
        [Display(Name = "Tên lớp")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Chưa điền mã lớp")]
        [Display(Name = "Mã lớp")]
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "Chưa chọn tên ngành")]
        [Display(Name = "Tên Ngành")]
        public int? MajorsId { get; set; }
        [Display(Name = "Tên Ngành")]
        public string MajorsName { get; set; }

        public string MajorsCode { get; set; }
        public SelectList MajorsList { get; set; } 
        public string Note { get; set; }
    }
}