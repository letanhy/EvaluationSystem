using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Models
{
    public class FacultyViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Chưa điền tên khoa")]
        [Display(Name = "Tên tên khoa")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Chưa điền mã khoa")]
        [Display(Name = "Mã khoa")]
        public string Code { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}