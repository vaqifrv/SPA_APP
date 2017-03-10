using System; 
using System.Collections.Generic; 
using System.Text;
using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle {
    
    
    public class ProfileDataMap : ClassMap<Profile> {
        
        public ProfileDataMap() {
			Table("PROFILE_DATA");
			LazyLoad();
			CompositeId().KeyProperty(x => x.User, "USERNAME")
			             .KeyProperty(x => x.PropertyValue, "PROPERTY_NAME");
			References(x => x.User).Column("USERNAME");
			References(x => x.Property).Column("PROPERTY_NAME");
			Map(x => x.PropertyValue).Column("PROPERTY_VALUE");
        }
    }
}
