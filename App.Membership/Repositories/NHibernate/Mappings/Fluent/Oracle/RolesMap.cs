using System; 
using System.Collections.Generic; 
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
    public class RoleMap : ClassMap<Role> {
        
        public RoleMap() {
			Table("ROLES");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Assigned().Column("ROLE_ID");
			Map(x => x.Name).Column("ROLE_NAME");
			Map(x => x.Description).Column("ROLE_DESC");

            HasManyToMany(x => x.Users).Cascade.All().Inverse().Table("UserRoles");

            HasManyToMany(x => x.Rights).Cascade.All().Table("RoleRights");
        }
    }
}
