using EvaluationSystem.Data;
using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var facultyListDb = _facultyRepository.ListAllInfo();
            var models = facultyListDb.Select(x => new FacultyViewModel
            {
                Name = x.Name,
                Code = x.Code,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                Id = x.Id
            });
            var a = facultyListDb.ToList();
            return View(models);
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
                int a = 54;
                //Faculty faculty = new Faculty();
                //faculty.Name = models.Name;
                //faculty.Code = models.Code;
                //faculty.CreatedDate = DateTime.Now;
                //_facultyRepository.Add(faculty);
                //if (faculty.Id > 0)
                //{
                //    return RedirectToAction("Index");
                //}
            }
            return View(models);
        }
        public ActionResult Edit(int Id)
        {
            var faculty = _facultyRepository.GetById(Id);
            FacultyViewModel model = new FacultyViewModel();
            if (faculty != null)
            {
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
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var faculty = _facultyRepository.GetById(id);
            if (faculty != null)
            {
                _facultyRepository.DeleteRs(faculty.Id);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}