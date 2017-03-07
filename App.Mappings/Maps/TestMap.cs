using App.Core.Domain;
using App.Core.Infrastructure;
using FluentNHibernate.Mapping;

namespace App.Mappings.Maps
{
    public class TestMap : ClassMap<Test>
    {
        public TestMap()
        {
            Table("Tests");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Custom<EntityGuidIdGenerator>().Not.Nullable().Column("Id").CustomSqlType("NVARCHAR(32)");

            Map(x => x.Name);
        }
    }
}
