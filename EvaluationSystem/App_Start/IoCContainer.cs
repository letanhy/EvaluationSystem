using Autofac;
using Autofac.Integration.Mvc;
using EvaluationSystem.Data;
using EvaluationSystem.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluationSystem.App_Start
{
    public class IoCContainer
    {
        public static void InitializeContainer()
        {
            // First we'll register the MVC/WCF stuff...
            var builder = new ContainerBuilder();
            // MVC - Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            // Scan an assembly for components (tên class repository nào cũng được)
            builder.RegisterAssemblyTypes(typeof(ClassRepository).Assembly)
                   .Where(t => t.FullName.StartsWith("EvaluationSystem.Data.Repositories") == true)
                   .AsImplementedInterfaces()
                   .WithParameter("context", new EvaluationSystemDbContext());
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}