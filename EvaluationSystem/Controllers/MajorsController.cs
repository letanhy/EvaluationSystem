﻿using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
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
            var majorsListDb = _majorsRepository.ListAllInfo();
            var models = majorsListDb.Select(x => new MajorsViewModel
            {
                Name = x.Name,
                Code = x.Code,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                FacultyCode = x.Faculty?.Code,
                FacultyName = x.Faculty?.Name,
                Id = x.Id,
            });
            return View(models);
        }
        public ActionResult Details(int Id)
        {
            var majors = _majorsRepository.GetById(Id);
            var model = new MajorsViewModel();
            if (majors != null)
            {
                model.Name = majors.Name;
                model.Code = majors.Code;
                model.CreatedDate = majors.CreatedDate;
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public void GetData(MajorsViewModel model)
        {
            var facultyListDb = _facultyRepository.ListAll();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "-- Chọn --",
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
                    return RedirectToAction("Index");
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
                    return RedirectToAction("Index");
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

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}