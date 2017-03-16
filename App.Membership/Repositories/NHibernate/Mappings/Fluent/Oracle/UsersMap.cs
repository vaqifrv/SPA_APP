using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle
{
    public class UserMap : ClassMap<User> {
        
        public UserMap() {
			Table("USERS");
			LazyLoad();
			Id(x => x.Username).Column("USERNAME").GeneratedBy.Assigned().CustomSqlType("NVARCHAR(32)");
			Map(x => x.Password).Column("PASSWORD").CustomSqlType("NVARCHAR(32)").Nullable();
			Map(x => x.IsEnabled).Column("IS_ENABLED");
			Map(x => x.IsDeleted).Column("IS_DELETED").Not.Nullable();
			Map(x => x.DisabledDate).Column("DISABLED_DATE").CustomSqlType("DATE");

            HasManyToMany(x => x.Roles).Cascade.All().Table("UserRoles");
        }
    }
}
