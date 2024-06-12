using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng điền đầy đủ họ tên sinh viên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Tuổi của sinh viên")]
        [Display(Name = "Tuổi")]
        public int? Age { get; set; }
        [Required(ErrorMessage = "Mã sinh viên")]
        [Display(Name = "MSSV")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Chọn lớp cho sinh viên")]
        [Display(Name = "Mã lớp")]
        public int? ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public int? MajorsId { get; set; }
        public string MajorsName { get; set; }
        public string MajorsCode { get; set; }
        public SelectList ClassList { get; set; }

    }
}