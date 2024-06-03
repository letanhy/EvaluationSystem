using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Models
{
    public class GradingPlanViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tên tiêu đề")]
        [Required(ErrorMessage = "Tên tiêu đề kế hoạch")]
        public string Titel { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [Required(ErrorMessage = "Ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Ngày kết thúc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
}