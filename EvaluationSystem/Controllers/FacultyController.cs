using EvaluationSystem.Data;
using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace EvaluationSystem.Controllers
{
    public class FacultyController : Controller
    {
        IFacultyRepository _facultyRepository;
        public FacultyController(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
            
        }
        // GET: Faculty
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult IndexGrid(string name, string code)
        {
            name = (name ?? "").Trim().ToLower();
            code = (code ?? "").Trim().ToLower();
            var models = _facultyRepository.GetAll().Where(x =>
                (name =="" || x.Name.Contains(name))
                && (code == "" || x.Code.Contains(code)))
                .AsNoTracking()
                .OrderBy(x => x.CreatedDate)
                .Select(x => new FacultyViewModel
                {
                    Name = x.Name,
                    Code = x.Code,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    Id = x.Id
                });
            return PartialView("_IndexGrid", models);
        }
        public ActionResult Create()
        {

            FacultyViewModel model = new FacultyViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(FacultyViewModel models)
        {
            if (ModelState.IsValid)
            {
                Faculty faculty = new Faculty();
                faculty.Name = models.Name;
                faculty.Code = models.Code;
                faculty.CreatedDate = DateTime.Now;
                _facultyRepository.Add(faculty);
                if (Request["IsPopup"] != null)
                {
                    return RedirectToAction("_ClosePopup", "Home",
                        new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                }
                return RedirectToAction("Index");
            }
            return View(models);
        }
        public ActionResult Edit(int Id)
        {
            var faculty = _facultyRepository.GetById(Id);
            if (faculty != null)
            {
                FacultyViewModel model = new FacultyViewModel();    
                model.Id = faculty.Id;
                model.CreatedDate = faculty.CreatedDate;
                model.Code = faculty.Code;
                model.Name = faculty.Name;
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(FacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var faculty = _facultyRepository.GetById(model.Id);
                if (faculty != null)
                {
                    faculty.Name = model.Name;
                    faculty.Code = model.Code;
                    faculty.ModifiedDate = DateTime.Now;
                    _facultyRepository.Update(faculty);

                    return RedirectToAction("_ClosePopup", "Home",
                        new { area = "", FunctionCallback = "ClosePopupAndReloadGrid" });
                }
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var faculty = _facultyRepository.GetById(id);
            if (faculty != null)
            {
                _facultyRepository.DeleteRs(faculty.Id);

                return Json(new { message = "Xóa thành công", type = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Xóa không thành công", type = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}