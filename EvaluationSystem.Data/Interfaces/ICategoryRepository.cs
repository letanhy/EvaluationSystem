﻿using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> ListAll();
        IEnumerable<Category> ListAllBySemester();
        IEnumerable<Category> ListAllByCourse();
        IEnumerable<Category> ListAllInfo();
        Category GetById(int Id);
        Category GetInfoById(int Id);
        int Add(Category category);
        void Update(Category category);
        void Delete(int Id);
        void DeleteRs(int Id);
    }
}
