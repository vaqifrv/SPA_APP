using System; 
using System.Collections.Generic; 
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
    public class UserMap : ClassMap<User> {
        
        public UserMap() {
			Table("USERS");
			LazyLoad();
			Id(x => x.Username).GeneratedBy.Assigned().Column("USERNAME");
			Map(x => x.Password).Column("PASSWORD").Not.Nullable();
			Map(x => x.IsEnabled).Column("IS_ENABLED");
			Map(x => x.IsDeleted).Column("IS_DELETED").Not.Nullable();
			Map(x => x.DisabledDate).Column("DISABLED_DATE");
        }
    }
}
