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
        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime? ModifiedDate { get; set; }
        [Required(ErrorMessage = "Chưa chọn tên ngành")]
        [Display(Name = "Tên Ngành")]
        public int? MajorsId { get; set; }
        [Display(Name = "Tên Ngành")]
        public string MajorsName { get; set; }
        [Display(Name = "Tên ngành")]
        public string MajorsCode { get; set; }
        [Display(Name = "Mã khoa")]
        public int? FacultyId { get; set; }
        [Display(Name = "Mã ngành")]
        public string FacultyName { get; set; }
        [Display(Name = "Mã ngành")]
        public string FacultyCode { get; set; }
        public SelectList MajorsList { get; set; } 
        public SelectList FacultyList { get; set; }
        [Display(Name = "Sỉ số lớp")]
        public int? CountStudent { get; set; }
        public List<StudentViewModel> StudentsList { get; set; }
        public double? ClassSize { get; set; }
    }
}