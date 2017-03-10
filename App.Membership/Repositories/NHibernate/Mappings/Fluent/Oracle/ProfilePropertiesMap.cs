using System; 
using System.Collections.Generic; 
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
    public class ProfilePropertyMap : ClassMap<ProfileProperty> {
        
        public ProfilePropertyMap() {
			Table("PROFILE_PROPERTIES");
			LazyLoad();
			Id(x => x.Name).GeneratedBy.Assigned().Column("NAME");
			Map(x => x.Label).Column("LABEL");
			Map(x => x.RegExp).Column("REGEXP");
			Map(x => x.ControlType).Column("CONTROL_TYPE");
			Map(x => x.CreatedDate).Column("CREATED_DATE");
			Map(x => x.OrderId).Column("ORDER_ID");
        }
    }
}
