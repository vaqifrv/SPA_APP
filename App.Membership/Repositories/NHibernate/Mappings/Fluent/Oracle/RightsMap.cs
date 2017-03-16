using App.Membership.Domain;
using FluentNHibernate.Mapping;

namespace App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle
{


    public class RighsMap : ClassMap<Right> {
        
        public RighsMap() {
			Table("RIGHTS");
			LazyLoad();
			Id(x => x.Id)
                .GeneratedBy.Identity()
                .Not.Nullable()
                .Column("RIGHT_ID")
                .CustomSqlType("INT");
			Map(x => x.Name).Column("RIGHT_NAME").CustomSqlType("NVARCHAR(32)");
			Map(x => x.Description).Column("RIGHT_DESC").CustomSqlType("NVARCHAR(256)");
			Map(x => x.LogEnabled).Column("LOG_ENABLED");

            HasManyToMany(x => x.Roles).Inverse().Cascade.All().Table("RoleRights");
        }
    }
}
