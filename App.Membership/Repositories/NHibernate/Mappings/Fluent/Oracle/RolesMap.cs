using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle
{


    public class RoleMap : ClassMap<Role> {
        
        public RoleMap() {
			Table("ROLES");
			LazyLoad();
			Id(x => x.Id)
                .GeneratedBy.Identity()
                .Not.Nullable()
                .Column("ROLE_ID")
                .CustomSqlType("INT");
			Map(x => x.Name).Column("ROLE_NAME").CustomSqlType("NVARCHAR(32)");
			Map(x => x.Description).Column("ROLE_DESC").CustomSqlType("NVARCHAR(256)");

            HasManyToMany(x => x.Users).Cascade.All().Inverse().Table("UserRoles");

            HasManyToMany(x => x.Rights).Cascade.All().Table("RoleRights");
        }
    }
}
