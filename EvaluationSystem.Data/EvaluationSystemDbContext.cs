using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace EvaluationSystem.Data
{
    public partial class EvaluationSystemDbContext : DbContext
    {
		public EvaluationSystemDbContext()
			: base("name=EvaluationSystemDbContext")
		{
			// this.Configuration.LazyLoadingEnabled = false;
			// this.Configuration.ProxyCreationEnabled = false;
		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Class> Classes { get; set; }
		public DbSet<Criteria> Criteria { get; set; }
		public DbSet<Majors> Majors { get; set; }
		public DbSet<Faculty> Faculty { get; set; }
		public DbSet<GradingPlan> GradingPlan { get; set; }
		public DbSet<Category> Category { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.AddFromAssembly(Assembly.GetAssembly(GetType())); //Current Assembly
			base.OnModelCreating(modelBuilder);
		}
	}
}
