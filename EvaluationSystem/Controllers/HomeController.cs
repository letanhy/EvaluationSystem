using EvaluationSystem.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Controllers
{
    public class HomeController : Controller
    {

        IStudentRepository _studentRepository;
        IClassRepository _classRepository;
        public HomeController(IStudentRepository studentRepository, IClassRepository classRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
        }
        // GET: Home
        public ActionResult Index()
        {
            var count = _studentRepository.GetCount();
            ViewBag.Count = count;
            return View();
        }

    }
}