using System;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;

namespace App.Repository.Infrastructure
{
    public static class CustomQueryOverExtensions
    {
        public static void Register()
        {
            try
            {
                ExpressionProcessor.RegisterCustomProjection(() => LTrim("", ""), ProcessLTrim);
            }
            catch (Exception ex)
            {

            }
            try
            {
                ExpressionProcessor.RegisterCustomProjection(() => NlsUpper(""), ProcessNlsUpper);

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toTrim">Trim olunacaq charecter</param>
        /// <returns></returns>
        public static string LTrim(this string value, string toTrim)
        {
            throw new Exception("Not to be used directly - use inside QueryOver expression");
        }

        internal static IProjection ProcessLTrim(MethodCallExpression methodCallExpression)
        {
            var property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            var toTrim = ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
            return Projections.SqlFunction("ltrim", NHibernateUtil.String, property, Projections.Constant(toTrim));
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="param">Nls Upper olunacaq string</param>
        /// <returns></returns>
        public static string NlsUpper(this string value)
        {
            throw new Exception("Not to be used directly - use inside QueryOver expression");
        }

        internal static IProjection ProcessNlsUpper(MethodCallExpression methodCallExpression)
        {
            var property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            return Projections.SqlFunction("NLS_UPPER", NHibernateUtil.String, property,
                Projections.Constant("NLS_SORT=XAZERBAIJANI"));
        }
    }
}