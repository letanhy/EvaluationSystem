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
        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool? IsDeleted { get; set; }
        [Required(ErrorMessage = "Chọn lớp cho sinh viên")]
        public int? ClassId { get; set; }
        [Display(Name = "Lớp")]
        public string ClassName { get; set; }
        [Display(Name = "Mã lớp")]
        public string ClassCode { get; set; }
        public int? MajorsId { get; set; }
        [Display(Name = "Ngành")]
        public string MajorsName { get; set; }
        [Display(Name = "Mã ngành")]
        public string MajorsCode { get; set; }
        public SelectList ClassList { get; set; }
        public int? FacultyId { get; set; }
        [Display(Name = "Khoa")]
        public string FacultyName { get; set; }
        [Display(Name = "Mã khoa")]
        public string FacultyCode { get; set; }

    }
}