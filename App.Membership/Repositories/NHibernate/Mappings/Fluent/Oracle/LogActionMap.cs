using System; 
using System.Collections.Generic; 
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
    public class LogActionMap : ClassMap<LogAction> {
        
        public LogActionMap() {
			Table("LOG_ACTIONS");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Assigned().Column("ACTION_ID");
			Map(x => x.Name).Column("NAME");
        }
    }
}
