using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Controllers
{
    public class MajorsController : Controller
    {
        IMajorsRepository _majorsRepository;
        IFacultyRepository _facultyRepository;
        public MajorsController(IMajorsRepository majorsRepository, IFacultyRepository facultyRepository)
        {
            _majorsRepository = majorsRepository;
            _facultyRepository = facultyRepository;
        }
        // GET: Majors
        public ActionResult Index()
        {
            var model = new MajorsViewModel();
            GetData(model);
            return View(model);
        }
        public PartialViewResult IndexGrid(string name, string code, int? facultyId)
        {
            name = (name ?? "").Trim().ToLower();
            code = (code ?? "").Trim().ToLower();
            facultyId = facultyId ?? 0;

            var models = _majorsRepository.GetAll()
                .Where(x =>
                (facultyId == 0 || x.FacultyId == facultyId)
                && (name == "" || x.Name.Contains(name))
                && (code == "" || x.Code.Contains(code))
                && (x.IsDeleted != true)
                )
                .Include(x => x.Faculty)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new MajorsViewModel
                {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                FacultyName = x.Faculty.Name,
            });

            return PartialView("_IndexGrid",models);
        }
        public ActionResult Details(int Id)
        {
            var majors = _majorsRepository.GetInfoById(Id);
            var model = new MajorsViewModel();
            if (majors != null)
            {
                model.Name = majors.Name;
                model.Code = majors.Code;
                model.CreatedDate = majors.CreatedDate;
                model.FacultyId = majors.FacultyId;
                model.FacultyName = majors.Faculty?.Name;
                model.FacultyCode = majors.Faculty?.Code;

                return View(model);
            }
            return RedirectToAction("Index");
        }
        public void GetData(MajorsViewModel model)
        {
            var facultyListDb = _facultyRepository.GetAll().OrderByDescending(x => x.CreatedDate);
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "-- Chọn khoa --",
                Value = null,
            });
            foreach (var item in facultyListDb)
            {
                var selectListItem = new SelectListItem();
                selectListItem.Text = item.Name;
                selectListItem.Value = item.Id.ToString();
                list.Add(selectListItem);
            }
            model.FacultyList = new SelectList(list, "Value", "Text");
        }
        public ActionResult Create()
        {
            MajorsViewModel model = new MajorsViewModel();
            ViewBag.Title = "Trang thêm ngành";
            GetData(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(MajorsViewModel models)
        {
            if (ModelState.IsValid)
            {
                Majors majors = new Majors();
                majors.Name = models.Name;
                majors.Code = models.Code;
                majors.CreatedDate = DateTime.Now;
                majors.FacultyId = models.FacultyId;
                _majorsRepository.Add(majors);
                if (majors.Id > 0)
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
            var majors = _majorsRepository.GetById(Id);
            MajorsViewModel model = new MajorsViewModel();
            if (majors != null)
            {
                model.Id = majors.Id;
                model.Code = majors.Code;
                model.Name = majors.Name;
                model.FacultyId = majors.FacultyId;
                GetData(model);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(MajorsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var majors = _majorsRepository.GetById(model.Id);
                if (majors != null)
                {
                    majors.Name = model.Name;
                    majors.Code = model.Code;
                    majors.ModifiedDate = DateTime.Now;
                    majors.FacultyId = model.FacultyId;
                    _majorsRepository.Update(majors);
                    return RedirectToAction("_ClosePopup", "Home",
                        new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                }
                return RedirectToAction("Index");
            }
            GetData(model);
            return View(model);
        }
        public ActionResult Delete(int Id)
        {
            var majors = _majorsRepository.GetById(Id);
            if (majors != null)
            {
                _majorsRepository.Delete(Id);

                return Json(new { message = "Xóa thành công", type = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Xóa không thành công", type = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}