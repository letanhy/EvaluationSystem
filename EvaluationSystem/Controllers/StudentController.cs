using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using EvaluationSystem.Data.Repositories;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Controllers
{
    public class StudentController : Controller
    {
        IStudentRepository _studentRepository;
        IClassRepository _classRepository;

        public StudentController(IStudentRepository studentRepository, IClassRepository classRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
        }
        // GET: Student
        public ActionResult Index()
        {
            
            return View();
        }
        public PartialViewResult IndexGrid()
        {
            var models = _studentRepository.GetAll()
                .Include(x => x.Class.Majors.Faculty)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new StudentViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Age = x.Age,
                Code = x.Code,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                MajorsId = x.Class.MajorsId,
                MajorsName = x.Class.Majors.Name,
                FacultyId = x.Class.Majors.FacultyId,
                FacultyName = x.Class.Majors.Name,
            });
            return PartialView("_IndexGrid", models);
        }
        public ActionResult Search(string searchTerm)
        {
            var students = _studentRepository.SearchStudents(searchTerm);
            var models = students.Select(x => new StudentViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Age = x.Age,
                Code = x.Code,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                ClassCode = x.Class.Code,
                MajorsId = x.Class.MajorsId,
                MajorsName = x.Class.Majors.Name,
                MajorsCode = x.Class.Majors.Code,
            }).ToList();
            return View("Index", models);
        }
        public ActionResult Details(int Id)
        {
            var student = _studentRepository.GetInfoById(Id);
            var model = new StudentViewModel();
            if (student != null)
            {
                model.Id = Id;
                model.FullName = student.FullName;
                model.Age = student.Age;
                model.Code = student.Code;
                model.ClassId = student.ClassId;
                model.ClassName = student.Class?.Name;
                model.ClassCode = student.Class?.Code;
                model.MajorsId = student.Class?.MajorsId;
                model.MajorsName = student.Class?.Majors?.Name;
                model.MajorsCode = student.Class?.Majors?.Code;
                model.FacultyId = student.Class?.Majors?.FacultyId;
                model.FacultyName = student.Class?.Majors?.Faculty?.Name;
                model.FacultyName = student.Class?.Majors?.Faculty?.Code;
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            StudentViewModel model = new StudentViewModel();

            GetData(model);
            return View(model);
        }
        public void GetData(StudentViewModel model)
        {
            var classListDb = _classRepository.ListAll();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "-- Chọn --",
                Value = null,
            });
            foreach (var item in classListDb)
            {
                /*var selectListItem = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                };*/
                var selectListItem = new SelectListItem();
                selectListItem.Text = item.Name;
                selectListItem.Value = item.Id.ToString();
                list.Add(selectListItem);
            }
            model.ClassList = new SelectList(list, "Value", "Text");
        }
        [HttpPost]
        public ActionResult Create(StudentViewModel models)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student();
                student.FullName = models.FullName;
                student.Age = models.Age;
                student.Code = models.Code;
                student.ClassId = models.ClassId;
                student.CreatedDate = DateTime.Now;
                student.IsDeleted = false;
                _studentRepository.Add(student);
                if (student.Id > 0)
                {
                    return RedirectToAction("_ClosePopup", "Home",
                        new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                }
            }
            GetData(models);
            return View(models);
        }
        public ActionResult Edit(int Id)
        {
            var student = _studentRepository.GetById(Id);
            StudentViewModel model = new StudentViewModel();
            if (student != null)
            {
                model.FullName = student.FullName;
                model.Age = student.Age;
                model.Code = student.Code;
                model.Id = student.Id;
                model.ClassId = student.ClassId;
                var classListDb = _classRepository.ListAll();
                var classList = classListDb.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
                GetData(model);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = _studentRepository.GetById(model.Id);
                if (student != null)
                {
                    student.FullName = model.FullName;
                    student.Age = model.Age;
                    student.Code = model.Code;
                    student.ClassId = model.ClassId;
                    student.ModifiedDate = DateTime.Now;
                    _studentRepository.Update(student);
                    return RedirectToAction("_ClosePopup", "Home",
                        new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                }
            }
            GetData(model);
            return View(model);
        }
        public ActionResult Delete(int Id)
        {
            var student = _studentRepository.GetById(Id);
            if (student != null)
            {
                _studentRepository.Delete(Id);

                return Json(new { message = "Xóa thành công", type = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Xóa không thành công", type = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}