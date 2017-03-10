using System;
using System.Collections.Generic;
using System.Linq;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;
using App.Membership.Repositories.Abstract;
using NHibernate;
using NHibernate.Criterion;

namespace App.Membership.Repositories.NHibernate
{
    public class LoggingRepository : RepositoryBase<Log, int>, ILoggingRepository
    {

        public ListResponse<Log> GetLogs(int startIndex, int pageSize, Services.Logging.LogSearchParams searchParams,
            out int totalItems)
        {
            try
            {
                var criteria = Session.CreateCriteria(typeof(Log));
                if (searchParams.BeginDate.HasValue)
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.Date >= searchParams.BeginDate));

                if (searchParams.EndDate.HasValue)
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.Date < searchParams.EndDate));

                if (searchParams.LogActionId.HasValue && searchParams.LogActionId != 0)
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.Action.Id == searchParams.LogActionId));

                if (searchParams.EventId.HasValue)
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.EventId == searchParams.EventId));

                if (!searchParams.IsAdmin)
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.Action.Id.IsIn(new[] { 2, 4, 5 })));

                if (!string.IsNullOrEmpty(searchParams.UserName))
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.UserName == searchParams.UserName));

                if (!string.IsNullOrEmpty(searchParams.ClientIp))
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.UserName == searchParams.UserName));

                var result = ((ICriteria)criteria.Clone())
                    //.AddOrder(new Order("DateCreated", false))
                    .SetFirstResult(startIndex)
                    .SetMaxResults(pageSize)
                    .List<Log>();

                totalItems = criteria
                    .SetProjection(Projections.Count(Projections.Id()))
                    .UniqueResult<int>();



                return new ListResponse<Log> { List = result?.ToList() };
            }
            catch (Exception ex)
            {
                totalItems = 0;
                return new ListResponse<Log> { Errors = new List<Exception> { ex } };
            }

            /*     if (filterParams.ContainsKey("Subject"))
                     criteria = criteria.Add(Expression.Like("Title", filterParams["Subject"].ToString().Replace('*', '%')));

                 if (filterParams.ContainsKey("Addressee"))
                     criteria = criteria.Add(Expression.Or(Expression.Like("Sender", filterParams["Addressee"].ToString().Replace('*', '%')), Expression.Like("Recipient", filterParams["Addressee"].ToString().Replace('*', '%'))));

                 switch (folderType)
                 {
                     case MessageFolder.Inbox:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Recipient", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.In("MessageStatus", new MessageStatus[] { MessageStatus.ReceivedNotRead, MessageStatus.ReceivedRead }));
                     case MessageFolder.Outbox:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Sender", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.ReadyToSend));
                     case MessageFolder.SentItems:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Sender", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.Sent));
                     case MessageFolder.Draft:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Sender", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.Draft));
                     case MessageFolder.Deleted:
                         return session.CreateCriteria(typeof(VMMSWebMail))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.Deleted));
                     case MessageFolder.ToExecute:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.ReceivedToExecute));
                     case MessageFolder.Executed:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Recipient", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.ReceivedExecuted));

                     default:
                         return null;
                 }*/
        }

        public ListResponse<Log> GetLogsForUser(string userName)
        {
            try
            {
                ICriteria criteria = Session.CreateCriteria(typeof(Log));

                if (!string.IsNullOrEmpty(userName))
                    criteria = criteria.Add(Restrictions.Where<Log>(x => x.UserName == userName));


                IList<Log> result = ((ICriteria)criteria.Clone()).List<Log>();

                return new ListResponse<Log> { List = result?.ToList() };

            }
            catch (Exception ex)
            {
                return new ListResponse<Log> { Errors = new List<Exception> { ex } };

            }
            /*     if (filterParams.ContainsKey("Subject"))
                     criteria = criteria.Add(Expression.Like("Title", filterParams["Subject"].ToString().Replace('*', '%')));

                 if (filterParams.ContainsKey("Addressee"))
                     criteria = criteria.Add(Expression.Or(Expression.Like("Sender", filterParams["Addressee"].ToString().Replace('*', '%')), Expression.Like("Recipient", filterParams["Addressee"].ToString().Replace('*', '%'))));

                 switch (folderType)
                 {
                     case MessageFolder.Inbox:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Recipient", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.In("MessageStatus", new MessageStatus[] { MessageStatus.ReceivedNotRead, MessageStatus.ReceivedRead }));
                     case MessageFolder.Outbox:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Sender", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.ReadyToSend));
                     case MessageFolder.SentItems:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Sender", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.Sent));
                     case MessageFolder.Draft:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Sender", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.Draft));
                     case MessageFolder.Deleted:
                         return session.CreateCriteria(typeof(VMMSWebMail))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.Deleted));
                     case MessageFolder.ToExecute:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.ReceivedToExecute));
                     case MessageFolder.Executed:
                         return criteria
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Recipient", addressee))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("Username", username))
                                 .Add(global::NHibernate.Criterion.Restrictions.Eq("MessageStatus", MessageStatus.ReceivedExecuted));

                     default:
                         return null;
                 }*/
        }

        public ListResponse<KeyValuePair<string, int>> GetTopUsedRights(DateTime? beginDate, DateTime? endDate)
        {

            //IList<MethodLog>  methodLog =from m in nSession.Query<Log>() 

            //    nSession.Query<Log>



            //ICriteria criteria = nSession.CreateCriteria(typeof(Log));
            //    criteria = criteria.Add(Expression.Where<Log>(x => x.Date >= beginDate));
            //    criteria = criteria.Add(Expression.Where<Log>(x => x.Date >= endDate));

            try
            {
                Log log = null;
                Right right = null;
                IQueryOver<Log, Log> query = Session.QueryOver(() => log)
                    .JoinAlias(() => log.Right, () => right, global::NHibernate.SqlCommand.JoinType.InnerJoin)
                    .Where(() => log.Right != null);

                if (beginDate.HasValue)
                    query.And(() => log.Date >= beginDate);

                if (endDate.HasValue)
                    query.And(() => log.Date < endDate);

                IList<object[]> test = query
                   .Select(
                        Projections.Group<Log>(x => x.Right),
                        Projections.Count<Log>(x => x.Id))
                    .OrderBy(Projections.Count<Log>(x => x.Id)).Desc
                    .Take(10)
                    .List<object[]>();
                //   .ListList<KeyValuePair<Right, int>>();
                // .List<object[]>();
                // IList<Dictionary<string, int>> dictionary = new List<Dictionary<string, int>>(test.Count);

                var result =
                    test.Select(key => new KeyValuePair<string, int>(((Right) key[0]).Name, (int) key[1])).ToList();
                return new ListResponse<KeyValuePair<string, int>> {List = result?.ToList()};
            }
            catch (Exception ex)
            {
                return new ListResponse<KeyValuePair<string, int>> { Errors = new List<Exception> { ex } };

            }
        }

    }
}

