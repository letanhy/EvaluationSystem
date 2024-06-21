using EvaluationSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationSystem.Data.Mappers
{
    internal class AccountMap : EntityTypeConfiguration<Account>
	{
		public AccountMap()
		{
			// Primary Key
			this.HasKey(t => new { t.Id });

			// Table & Column Mappings
			this.ToTable("Account");
		}
	}
}
