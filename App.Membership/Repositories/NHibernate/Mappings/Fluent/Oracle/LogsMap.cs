using System;
using System.Collections.Generic;
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;
using App.Core.Infrastructure;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
    public class LogMap : ClassMap<Log> {
        
        public LogMap() {
			Table("LOGS");
			LazyLoad();
			Id(x => x.Id)
                .GeneratedBy.Custom<EntityGuidIdGenerator>()
                .Not.Nullable()
                .Column("LOG_ID")
                .CustomSqlType("NVARCHAR2(32)");
			References(x => x.Action).Column("ACTION_ID");
			Map(x => x.LogActionResult).Column("ACTION_RESULT");
			Map(x => x.Level).Column("LOG_LEVEL");
			Map(x => x.Description).Column("DESCRIPTION");
			Map(x => x.EventId).Column("EVENT_ID");
			Map(x => x.Date).Column("LOG_DATE");
			Map(x => x.ClientIp).Column("CLIENT_IP");
			Map(x => x.UserAgent).Column("USER_AGENT");
			Map(x => x.UserName).Column("USERNAME");
        }
    }
}
