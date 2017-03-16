using System;
using System.Collections.Generic;
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;
using App.Core.Infrastructure;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
   // public class LogActionMap : ClassMap<LogAction> {
        
   //     public LogActionMap() {
			//Table("LOG_ACTIONS");
			//LazyLoad();
			//Id(x => x.Id)
   //             .GeneratedBy.Custom<EntityGuidIdGenerator>()
   //             .Not.Nullable()
   //             .Column("ACTION_ID")
   //             .CustomSqlType("NVARCHAR(32)");
			//Map(x => x.Name).Column("NAME").CustomSqlType("NVARCHAR(32)");
   //     }
   // }
}
