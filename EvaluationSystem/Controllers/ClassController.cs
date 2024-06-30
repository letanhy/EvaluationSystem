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
using Newtonsoft.Json;

namespace EvaluationSystem.Controllers
{
    public class ClassController : Controller
    {
        IClassRepository _classRepository;
        IMajorsRepository _majorsRepository;
        IStudentRepository _studentRepository;
        IFacultyRepository _facultyRepository;
        public ClassController( IFacultyRepository facultyRepository, IClassRepository classRepository, IMajorsRepository majorsRepository, IStudentRepository studentRepository)
        {
            _classRepository = classRepository;
            _majorsRepository = majorsRepository;
            _studentRepository = studentRepository;
            _facultyRepository = facultyRepository;
        }
        public ActionResult Index()
        {
            var model = new ClassViewModel();
            var majorsGroupList = _majorsRepository.GetAll().OrderByDescending(x => x.CreatedDate);
            model.MajorsList = GetSelectList(majorsGroupList, "Id", "Name", "", "--Ngành--");
            var facultyGroupList = _facultyRepository.GetAll().OrderByDescending(x => x.CreatedDate);
            model.FacultyList = GetSelectList(facultyGroupList, "Id", "Name", "", "--Khoa--");
            return View(model);
        }
        public PartialViewResult IndexGrid(string name, string code, int? majorsId, int? facultyId)
        {
            majorsId = majorsId ?? 0;
            name = (name ?? "").Trim().ToLower();
            code = (code ?? "").Trim().ToLower();
            facultyId = facultyId ?? 0;

            var _class = _classRepository.ListAll().Where(x =>
                (majorsId == 0 || x.MajorsId == majorsId)
                && (facultyId == 0 || x.Majors.FacultyId == facultyId)
                && (name == "" || x.Name.Contains(name))
                && (code == "" || x.Code.Contains(code))
                );
            var models = _class
                .Include(x=>x.Majors.Faculty)
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

        // GET: Class/Details/5
        public ActionResult Detail(int Id)
        {
            var _class = _classRepository.GetInfoById(Id);
            var model = new ClassViewModel();
            if (_class != null)
            {
                model.Id = _class.Id;
                model.Name = _class.Name;
                model.Code = _class.Code;
                model.CreatedDate = _class.CreatedDate;
                model.MajorsId = _class.MajorsId;
                model.MajorsName = _class.Majors?.Name;
                model.MajorsCode = _class.Majors?.Code;
                model.FacultyId = _class.Majors?.FacultyId;
                model.FacultyCode = _class.Majors?.Faculty?.Code;
                model.FacultyName = _class.Majors?.Faculty?.Name;
                model.CountStudent = _studentRepository.CountStudent(Id);

                return View(model);
            }
            return RedirectToAction("Index");
        }
        public PartialViewResult DetailGrid(int classId)
        {
            var models = _studentRepository.GetStudentbyClassId(classId)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new StudentViewModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Code = x.Code,
                    BirthDate = x.BirthDate,
                    ClassName = x.Class.Name,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate
                });

            return PartialView("_DetailGrid", models);
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
