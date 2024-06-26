﻿using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Interfaces
{
    public interface IStudentRepository
    {
        IQueryable<Student> GetAll();
        IQueryable<Student> GetStudentbyClassId(int classId);
        Student GetById(int Id);
        Student GetInfoById(int Id);
        int Add(Student student);
        void Update(Student student);
        void Delete(int Id);
        void DeleteRs(int Id);
        IEnumerable<Student> ListAllInfo();
        int GetCount();
        int CountStudent(int ClassId);
        IQueryable<Student> GetStudents(int classId);
        IQueryable<Student> SearchStudents(string searchTerm);
    }
}
