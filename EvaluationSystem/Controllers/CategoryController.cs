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
            var categoryListDb = _categoryRepository.ListAllBySemester();
            var models = categoryListDb.Select(x => new CategoryViewModel
            {
                Name = x.Name,
                Code = x.Code,
                OrderNo = x.OrderNo,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                Id = x.Id
            });
            return View(models);
        }
        public ActionResult IndexCourse()
        {
            var categoryListDb = _categoryRepository.ListAllByCourse();
            var models = categoryListDb.Select(x => new CategoryViewModel
            {
                Name = x.Name,
                Code = x.Code,
                OrderNo = x.OrderNo,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                Id = x.Id
            });
            return View(models);
        }
        public ActionResult CreateSemester()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateSemester(CategoryViewModel models)
        {
            Category category = new Category();
            category.Name = models.Name;
            category.Code = models.Code;
            category.Group = "Hocky";
            category.OrderNo = models.OrderNo;
            category.CreatedDate = DateTime.Now;
            category.IsDeleted = false;
            _categoryRepository.Add(category);
            if (category.Id > 0)
            {
                return RedirectToAction("IndexSemester");
            }
            return View(models);
        }
        public ActionResult CreateCourse()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateCourse(CategoryViewModel models)
        {
            Category category = new Category();
            category.Name = models.Name;
            category.Code = models.Name;
            category.Group = "Khoahoc";
            category.OrderNo = models.OrderNo;
            category.CreatedDate = DateTime.Now;
            category.IsDeleted = false;
            _categoryRepository.Add(category);
            if (category.Id > 0)
            {
                return RedirectToAction("IndexCourse");
            }
            return View(models);
        }
        public ActionResult EditSemester(int Id)
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
        public ActionResult EditSemester(CategoryViewModel model)
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
        public ActionResult EditCourse(int Id)
        {
            var category = _categoryRepository.GetById(Id);
            CategoryViewModel model = new CategoryViewModel();
            if (category != null)
            {
                model.Name = category.Name;
                model.Code = category.Code;
                return View(model);
            }
            return RedirectToAction("IndexCourse");
        }
        [HttpPost]
        public ActionResult EditCourse(CategoryViewModel model)
        {

            var category = _categoryRepository.GetById(model.Id);
            if (category != null)
            {
                category.Name = model.Name;
                category.Code = model.Code;
                category.ModifiedDate = DateTime.Now;
                _categoryRepository.Update(category);
                return RedirectToAction("IndexCourse");
            }
            return View(model);
        }
        public ActionResult DeleteSemester(int Id)
        {
            var category = _categoryRepository.GetById(Id);
            if (category != null)
            {
                _categoryRepository.DeleteRs(Id);

                return RedirectToAction("IndexSemester");
            }
            return RedirectToAction("IndexSemester");
        }
        public ActionResult DeleteCourse(int Id)
        {
            var category = _categoryRepository.GetById(Id);
            if (category != null)
            {
                _categoryRepository.DeleteRs(Id);

                return RedirectToAction("IndexCourse");
            }
            return RedirectToAction("IndexCourse");
        }
    }
}