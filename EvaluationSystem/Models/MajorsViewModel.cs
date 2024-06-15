using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Models
{
    public class MajorsViewModel 
    {

        public int Id { get; set; }
        [Display(Name = "Tên ngành")]
        [Required(ErrorMessage = "Chưa điền tên ngành")]
        public string Name { get; set; }
        [Display(Name = "Mã ngành")]
        [Required(ErrorMessage = "Chưa điền mã ngành")]
        public string Code { get; set; }
        [Display(Name = "Ngày")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Ngày")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "Khoa")]
        [Required(ErrorMessage = "Chưa chọn khoa cho ngành")]
        public int? FacultyId { get; set; }
        [Display(Name = "Khoa")]
        public string FacultyName { get; set; }
        [Display(Name = "Mã khoa")]
        public string FacultyCode   { get; set; }
        public SelectList FacultyList   { get; set; }

    }
}