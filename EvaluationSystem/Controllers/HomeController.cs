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
        IMajorsRepository _majorsRepository;
        IFacultyRepository _facultyRepository;
        public HomeController(IFacultyRepository facultyRepository, IStudentRepository studentRepository, IClassRepository classRepository, IMajorsRepository majorsRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            _majorsRepository = majorsRepository;
            _facultyRepository = facultyRepository;
        }
        // GET: Home
        public ActionResult Index()
        {
            var countClass = _classRepository.GetCount();
            var countStudents = _studentRepository.GetCount();
            var countMajors = _majorsRepository.GetCount();
            var countFaculty = _facultyRepository.GetCount();
            ViewBag.CountStudents = countStudents;
            ViewBag.CountClass = countClass;
            ViewBag.CountMajors = countMajors;
            ViewBag.CountFaculty = countFaculty;
            return View();
        }
        public ActionResult _ClosePopup()
        {
            ViewBag.ClosePopup = TempData["SuccessMessage"];
            return View();
        }

    }
}