using EvaluationSystem.Data.Entities;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvaluationSystem.Data.Repositories;
using EvaluationSystem.Data.Interfaces;
using System.Data.Entity;

namespace EvaluationSystem.Controllers
{
    public class ClassController : Controller
    {
        IClassRepository _classRepository;
        IMajorsRepository _majorsRepository;
        IStudentRepository _studentRepository;
        public ClassController(IClassRepository classRepository, IMajorsRepository majorsRepository, IStudentRepository studentRepository)
        {
            _classRepository = classRepository;
            _majorsRepository = majorsRepository;
            _studentRepository = studentRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult IndexGrid()
        {
            var models = _classRepository.ListAll()
                .Include(x=>x.Majors)
                .AsNoTracking()
                .OrderByDescending(x=>x.CreatedDate)
                .Select(x => new ClassViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    CreatedDate = x.CreatedDate,
                    MajorsCode = x.Majors.Code,
                    MajorsName = x.Majors.Name,
                });

            return PartialView("_IndexGrid", models);
        }

        // GET: Class/Details/5
        public ActionResult Details(int Id)
        {
            var _class = _classRepository.GetInfoById(Id);
            var model = new ClassViewModel();
            if (_class != null)
            {
                model.Name = _class.Name;
                model.Code = _class.Code;
                model.CreatedDate = _class.CreatedDate;
                model.MajorsName = _class.Majors?.Name;
                model.MajorsCode = _class.Majors?.Code;
                model.CountStudent = _studentRepository.CountStudent(Id);
                model.StudentsList = _studentRepository
                    .GetStudents(Id)
                    .Select(x => new StudentViewModel
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
                GetData(model);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // GET: Class/Create
        public ActionResult Create()
        {
            ClassViewModel model = new ClassViewModel();
            GetData(model);
            return View(model);
        }
        public void GetData(ClassViewModel model)
        {
            var majorsListDb = _majorsRepository.GetAll();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "-- Chọn --",
                Value = null,
            });
            foreach (var item in majorsListDb)
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
            model.MajorsList = new SelectList(list, "Value", "Text");
        }
        // POST: Class/Create
        [HttpPost]
        public ActionResult Create(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                Class _class = new Class();
                _class.Code = model.Code;
                _class.Name = model.Name;
                _class.CreatedDate = DateTime.Now;
                _class.MajorsId = model.MajorsId;
                _classRepository.Add(_class);
                if (_class.Id>0)
                {
                    if (Request["IsPopup"] != null)
                    {
                        return RedirectToAction("_ClosePopup", "Home",
                            new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                    }
                    return RedirectToAction("Index");
                }
            }
            GetData(model);
            return View(model);
        }

        // GET: Class/Edit/5
        public ActionResult Edit(int id)
        {
            var _class = _classRepository.GetById(id);
            ClassViewModel model = new ClassViewModel();
            if (_class != null)
            {
                model.Id = _class.Id;
                model.CreatedDate = _class.CreatedDate;
                model.Code = _class.Code;
                model.Name = _class.Name;
                model.MajorsId = _class.MajorsId;
                GetData(model);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
 
        }

        // POST: Class/Edit/5
        [HttpPost]
        public ActionResult Edit(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _class = _classRepository.GetById(model.Id);
                if (_class != null)
                {
                    _class.Name = model.Name;
                    _class.Code = model.Code;
                    _class.MajorsId = model.MajorsId;
                    _class.ModifiedDate = DateTime.Now;
                    _classRepository.Update(_class);
                    return RedirectToAction("_ClosePopup", "Home",
                        new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                }
            }
            GetData(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            var _class = _classRepository.GetById(id);
            if (_class != null)
            {
                _classRepository.Delete(_class);

                return Json(new { message = "Xóa thành công", type = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Xóa không thành công", type = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}
