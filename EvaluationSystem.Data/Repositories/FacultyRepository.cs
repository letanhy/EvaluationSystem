using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluationSystem.Data.Interfaces;
using System.Data.SqlClient;

namespace EvaluationSystem.Data.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        EvaluationSystemDbContext context = new EvaluationSystemDbContext();
        public void Add(Faculty model)
        {
            var namePr = new SqlParameter("@Name", model.Name);
            var codePr = new SqlParameter("@Code", model.Code);
            var createdDatePr = new SqlParameter("@CreatedDate", model.CreatedDate);
            context.Database.ExecuteSqlCommand("Sp_Faculty_Add @Name, @Code, @CreatedDate", namePr, codePr, createdDatePr);

            //var namePr = new SqlParameter("@Name", model.Name);
            //var CodePr = new SqlParameter("@Code", model.Code);
            //var CreatedDatePr = new SqlParameter("@CreatedDate", model.CreatedDate);
            //context.Database.ExecuteSqlCommand("EXEC Sp_Faculty_Add @Name, @Code, @CreatedDate", namePr, CodePr, CreatedDatePr);

            //context.Faculty.Add(model);
            //context.SaveChanges();
            //return model.Id;
        }
        public void Update(Faculty model)
        {
            
            var IdPr = new SqlParameter("@Id", model.Id);
            var namePr = new SqlParameter("@Name", model.Name);
            var CodePr = new SqlParameter("@Code", model.Code);
            var ModifiedDatePr = new SqlParameter("@ModifiedDate", model.ModifiedDate);
            context.Database.ExecuteSqlCommand("UpdateFaculty @Id, @Name, @Code, @ModifiedDate", IdPr, namePr, CodePr, ModifiedDatePr);
          
        }
        public void Delete(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            context.Faculty.Remove(model);
            context.SaveChanges();
        }
        public void Delete(Faculty faculty)
        {
            context.Faculty.Remove(faculty);
            context.SaveChanges();
        }
        public void DeleteRs(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            model.IsDeleted = true;
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
        public IEnumerable<Faculty> ListAll()
        {

            return context.Faculty;
        }
        public IEnumerable<Faculty> ListAllInfo()
        {
            var result = context.Database.SqlQuery<Faculty>("GetALL");
            return result;
        }
        
        public Faculty GetById(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            return model;
        }
        public Faculty GetInfoById(int Id)
        {
            var model = context.Faculty.SingleOrDefault(x => x.Id == Id);
            return model;
        }

        public int GetCount()
        {
            return context.Faculty.Count(x => x.IsDeleted != true);
        }
    }
}
