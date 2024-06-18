﻿using EvaluationSystem.Data.Entities;
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
        IMajorsRepository _majorsRepository;
        IFacultyRepository _facultyRepository;

        public StudentController(IStudentRepository studentRepository
            , IClassRepository classRepository, IFacultyRepository facultyRepository, IMajorsRepository majorsRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            _facultyRepository = facultyRepository;
            _majorsRepository = majorsRepository;
        }
        // GET: Student
        public ActionResult Index()
        {
            var model = new StudentViewModel();
            var classGroupList = _classRepository.ListAll().OrderByDescending(x => x.CreatedDate);
            model.ClassList = GetSelectList(classGroupList, "Id", "Name", "", "--Lớp--");
            var majorsGroupList = _majorsRepository.GetAll().OrderByDescending(x => x.CreatedDate);
            model.MajorsList = GetSelectList(majorsGroupList, "Id", "Name", "", "--Chuyên ngành--");
            var facultyGroupList = _facultyRepository.GetAll().OrderByDescending(x => x.CreatedDate);
            model.FacultyList = GetSelectList(facultyGroupList, "Id", "Name", "", "--Khoa--");
            return View(model);
        }
        public PartialViewResult IndexGrid(string fullName, string code, int? classId, int? majorsId, int? facultyId)
        {
            classId = classId ?? 0;
            majorsId = majorsId ?? 0;
            fullName = (fullName ?? "").Trim().ToLower();
            code = (code ?? "").Trim().ToLower();
            facultyId = facultyId ?? 0;

            var students = _studentRepository.GetAll().Where(x =>
                (classId == 0 || x.ClassId == classId)
                && (majorsId == 0 || x.Class.MajorsId == majorsId)
                && (facultyId == 0 || x.Class.Majors.FacultyId == facultyId)
                && (fullName == "" || x.FullName.Contains(fullName))
                && (code == "" || x.Code.Contains(code))
                );

            var models = students
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
                FacultyName = x.Class.Majors.Faculty.Name,
            });
            return PartialView("_IndexGrid", models);
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
        protected SelectList GetSelectList(IQueryable<object> list, string value, string text, object selected = null, string NullOrNameEmpty = null)
        {
            var selectList = new SelectList(list, value, text).ToList();
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectList.Insert(0, itemEmpty);
            }
            return new SelectList(selectList, "Value", "Text", selected);
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
                    if (Request["IsPopup"] != null)
                    {
                        return RedirectToAction("_ClosePopup", "Home",
                            new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                    }
                    return RedirectToAction("Index");
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

        [HttpGet]
        public JsonResult GetMajorsByFacultyId(int? facultyId)
        {
            var model = new StudentViewModel();
            var majorsGroupList = _majorsRepository.GetAll()
                .Where(x => (facultyId == 0 || x.FacultyId == facultyId))
                .OrderByDescending(x => x.CreatedDate);
            model.MajorsList = GetSelectList(majorsGroupList, "Id", "Name", "", "--Tất cả--");
            var majorsList = model.MajorsList;
            return Json(new { isSuccess = true, data = majorsList }, JsonRequestBehavior.AllowGet);
        }
    }
}