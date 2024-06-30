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
    public class CategoryController : Controller
    {
        ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // GET: Category
        public ActionResult IndexSemester()
        {
            ViewBag.Group = "hocky";
            return View();
        }
        public ActionResult IndexCourse()
        {
            ViewBag.Group = "khoahoc";
            return View();
        }
        public ActionResult IndexGrid(string group)
        {
            var models = _categoryRepository
                .GetAll()
                .Where(x => x.Group == group)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Group = x.Group,
                    OrderNo = x.OrderNo,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate
                });

            return PartialView("_IndexGrid", models);
        }
            public ActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            Category category = new Category();
            category.Name = model.Name;
            category.Code = model.Code;
            category.Group = model.Group;
            category.OrderNo = model.OrderNo;
            category.CreatedDate = DateTime.Now;
            category.IsDeleted = false;
            _categoryRepository.Add(category);
            if (category.Id > 0)
            {
                if (category.Group == "Hocky")
                {
                    return RedirectToAction("IndexSemester");
                }
                return RedirectToAction("IndexCourse");
            }
            return View(model);
        }
        public ActionResult Edit(int Id)
        {
            var category = _categoryRepository.GetById(Id);
            CategoryViewModel model = new CategoryViewModel();
            if (category != null)
            {
                model.Name = category.Name;
                model.Code = category.Code;
                return View(model);
            }
            return RedirectToAction("IndexSemester");
        }
        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            
            var category = _categoryRepository.GetById(model.Id);
            if (category != null)
            {
                category.Name = model.Name;
                category.Code = model.Code;
                category.ModifiedDate = DateTime.Now;
                _categoryRepository.Update(category);
                return RedirectToAction("IndexSemester");
            }
            return View(model);
        }
        public ActionResult Delete(int Id)
        {
            var category = _categoryRepository.GetById(Id);
            if (category != null)
            {
                _categoryRepository.DeleteRs(Id);

                return RedirectToAction("IndexSemester");
            }
            return RedirectToAction("IndexSemester");
        }
    }
}