using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using EvaluationSystem.Data.Repositories;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.Controllers
{
    public class GradingPlanController : Controller
    {
        IGradingPlanRepository _gradingPlanRepository;

        public GradingPlanController(IGradingPlanRepository gradingPlanRepository)
        {
            _gradingPlanRepository = gradingPlanRepository;
        }
        // GET: Student
        public ActionResult Index()
        {
            var gradingPlanListDb = _gradingPlanRepository.ListAllInfo();
            var models = gradingPlanListDb.Select(x => new GradingPlanViewModel
            {
                Id = x.Id,
                Titel = x.Titel,
                Note = x.Note,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
            });
            return View(models);
        }
        public ActionResult Details(int Id)
        {
            var gradingPlan = _gradingPlanRepository.GetInfoById(Id);
            var model = new GradingPlanViewModel();
            if (gradingPlan != null)
            {
                model.Id = Id;
                model.Titel = gradingPlan.Titel;
                model.Note = gradingPlan.Note;
                model.StartDate = gradingPlan.StartDate;
                model.EndDate = gradingPlan.EndDate;
                model.CreatedDate = gradingPlan.CreatedDate;
                model.ModifiedDate = gradingPlan.ModifiedDate;
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            GradingPlanViewModel model = new GradingPlanViewModel();
            return View(model);
        }
       
        [HttpPost]
        public ActionResult Create(GradingPlanViewModel models)
        {
            if (ModelState.IsValid)
            {
                GradingPlan gradingPlan = new GradingPlan();
                gradingPlan.Titel = models.Titel;
                gradingPlan.Note = models.Note;
                gradingPlan.StartDate = models.StartDate;
                gradingPlan.EndDate = models.EndDate;
                gradingPlan.CreatedDate = DateTime.Now;
                _gradingPlanRepository.Add(gradingPlan);
                if (gradingPlan.Id > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(models);
        }
        public ActionResult Edit(int Id)
        {
            var gradingPlan = _gradingPlanRepository.GetById(Id);
            GradingPlanViewModel model = new GradingPlanViewModel();
            if (gradingPlan != null)
            {
                model.Titel = gradingPlan.Titel;
                model.Note = gradingPlan.Note;
                model.StartDate = gradingPlan.StartDate;
                model.EndDate = gradingPlan.EndDate;
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(GradingPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gradingPlan = _gradingPlanRepository.GetById(model.Id);
                if (gradingPlan != null)
                {
                    gradingPlan.Titel = model.Titel;
                    gradingPlan.Note = model.Note;
                    gradingPlan.StartDate = model.StartDate;
                    gradingPlan.EndDate = model.EndDate;
                    gradingPlan.ModifiedDate = DateTime.Now;
                    _gradingPlanRepository.Update(gradingPlan);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
            
        public ActionResult Delete(int Id)
        {
            var gradingPlan = _gradingPlanRepository.GetById(Id);
            if (gradingPlan != null)
            {
                _gradingPlanRepository.Delete(Id);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}