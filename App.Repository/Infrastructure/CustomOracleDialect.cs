using System.Data;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;

namespace App.Repository.Infrastructure
{
    public class CustomOracleDialect : Oracle10gDialect
    {
        protected override void RegisterFunctions()
        {
            base.RegisterFunctions();
            RegisterFunction("NLS_UPPER", new StandardSQLFunction("NLS_UPPER"));
            RegisterColumnType(DbType.Boolean, "NUMBER(1,0)");
        }
    }
}