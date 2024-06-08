using EvaluationSystem.Data.Entities;
using EvaluationSystem.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();
        public int Add(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();

            return student.Id;
        }

        public void Delete(int Id)
        {
            var student = context.Students.SingleOrDefault(x => x.Id == Id);
            context.Students.Remove(student);
            context.Entry(student).State = EntityState.Deleted;
            context.SaveChanges();
        }


        public void DeleteRs(int Id)
        {
            var student = context.Students.SingleOrDefault(x => x.Id == Id);
            student.IsDeleted = true;
            context.Entry(student).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Student GetById(int Id)
        {
            return context.Students.SingleOrDefault(x => x.Id == Id);
        }

        public IQueryable<Student> ListAll()
        {
            return context.Students.Where(x => x.IsDeleted != true);
        }

        public IEnumerable<Student> ListAllInfo()
        {
            return context.Students.Include(x => x.Class.Majors).Where(x => x.IsDeleted != true);
        }

        public void Update(Student student)
        {
            context.Entry(student).State = EntityState.Modified;
            context.SaveChanges();
        }
        public Student GetInfoById(int Id)
        {
            return context.Students.Include(x => x.Class.Majors).SingleOrDefault(x => x.Id == Id);
        }

        public int GetCount()
        {
            return context.Students.Count(x => x.IsDeleted != true);
        }

        public int CountStudent(int classId)
        {
            return context.Students.Count(x => x.ClassId == classId && x.IsDeleted != true);
        }

        public IQueryable<Student> GetStudents(int classId)
        {
            return context.Students.Where(x => x.ClassId == classId && x.IsDeleted != true);
        }

        public IQueryable<Student> SearchStudents(string searchTerm)
        {
            return context.Students
                .Where(x => (string.IsNullOrEmpty(searchTerm) || x.FullName.Contains(searchTerm) || x.Code.Contains(searchTerm))
                && x.IsDeleted != true);
        }
    }
}
