using System; 
using System.Collections.Generic; 
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
    public class RighsMap : ClassMap<Right> {
        
        public RighsMap() {
			Table("RIGHTS");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Assigned().Column("RIGHT_ID");
			Map(x => x.Name).Column("RIGHT_NAME");
			Map(x => x.Description).Column("RIGHT_DESC");
			Map(x => x.LogEnabled).Column("LOG_ENABLED");

            HasManyToMany(x => x.Roles).Inverse().Cascade.All().Table("RoleRights");
        }
    }
}
