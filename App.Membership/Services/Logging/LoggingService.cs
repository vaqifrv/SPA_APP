using System;
using System.Collections.Generic;
using App.Membership.Domain;
using App.Membership.Repositories;
using App.Membership.Repositories.Abstract;
using App.Membership.Services.Logging.Enums;

namespace App.Membership.Services.Logging
{

    public class LoggingService
    {

        public static LoggingService Current = new LoggingService();
        private readonly ILogContextProvider _logContextProvider;
        private readonly ILoggingRepository _loggingRepository;
        private readonly ILogActionRepository _logActionRepository;
        private readonly IRightRepository _rightRepository;


        public LoggingService()
        {

            _logContextProvider = LogContextProviderFactory.GetLogContextProvider();

            //Hardcoded
            _loggingRepository = RepositoryFactory.GetLoggingRepository();
            _rightRepository = RepositoryFactory.GetRightRepository();
            _logActionRepository = RepositoryFactory.GetLogActionRepository();
            //
        }


        #region WriteLog

        public int WriteLogActionLogOut()
        {
            LogData logData = new LogData()
            {
                ActionId = 5,   //Logout 
                Level = Level.Info,
                LogActionResult = LogActionResult.Success,
                EventId = 12   // User was logged out.
            };

            return Current.WriteLog(logData);
        }

        public int WriteLogActionLogin(string userName, LogActionResult result)
        {
            LogData logData = new LogData()
            {
                ActionId = 4,   //Login    
                Level = Level.Info
            };

            if (result == LogActionResult.Success)
            {
                logData.LogActionResult = LogActionResult.Success;
                logData.EventId = 10;   // User login was successful.
                logData.Description = "username : " + userName;
            }
            else
            {
                logData.LogActionResult = LogActionResult.Failed;
                logData.EventId = 11;   // User login was failed.
                logData.Description = "Tried to login with username : " + userName;
            }

            return Current.WriteLog(logData, userName);
        }

        public int WriteLog(LogData logData)
        {
            return WriteLog(logData, string.Empty);
        }


        /// <summary>
        /// Writes log and returns the identity of the inserted record.
        /// </summary>
        /// <param name="logData"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private int WriteLog(LogData logData, string userName)
        {
            Right right = null;
            if (!string.IsNullOrEmpty(logData.RightName))
            {
                var findByNameResult = _rightRepository.FindByName(logData.RightName);
                if (!findByNameResult.Success)
                    throw findByNameResult.Errors[0];
                right = findByNameResult.Value;
                if (right == null)
                    throw new Exception($"Right not found. Please create right for \"{logData.RightName}\"");
                if (!right.LogEnabled)
                    return -1;
            }

            if ((logData.LogActionResult == LogActionResult.NotSet) || (logData.Level == Level.NotSet))
            {
                throw new ArgumentNullException($"logData has some invalid fields");
            }

            LoggingContext loggingContext = _logContextProvider.GetLogContext();
            if (loggingContext == null)
            {
                throw new NullReferenceException("LoggingContext is null");
            }

            var logAction = _logActionRepository.FindBy(logData.ActionId);
            if (logAction.Value == null) { throw new NullReferenceException("Log action with such Id was not found"); }  //////////////



            var newLog = new Log
            {
                ClientIp = loggingContext.ClientIp,
                Date = loggingContext.RequestDate,
                UserAgent = loggingContext.UserAgent,
                UserName = loggingContext.UserName,

                Action = logAction.Value,
                LogActionResult = logData.LogActionResult,
                Description = logData.Description,
                EventId = logData.EventId,
                Level = logData.Level,
                Right = right
            };

            //for login 
            if (string.IsNullOrEmpty(newLog.UserName) && !string.IsNullOrEmpty(userName))
                newLog.UserName = userName;

            _loggingRepository.Add(newLog);
            return newLog.Id;
        }

        #endregion

        #region GetLogs

        public Log GetLog(int logId)
        {
            return _loggingRepository.FindBy(logId).Value;
        }

        public IList<Log> GetLogs(int startIndex, int pageSize, LogSearchParams searchParams, out int totalItems)
        {
            var list = _loggingRepository.GetLogs(startIndex, pageSize, searchParams, out totalItems).List;
            return list;
        }

        public IList<KeyValuePair<string, int>> GetTopUsedRights(DateTime? beginDate, DateTime? endDate)
        {
            return _loggingRepository.GetTopUsedRights(beginDate, endDate).List;
        }


        #endregion

        #region DeleteLogs

        //public int DeleteLogs(int fromIndex, int number)
        //{
        //    throw new NotImplementedException();
        //}

        //public int DeleteLogs(int fromIndex, int number, LogStatus logStatus)
        //{
        //    throw new NotImplementedException();
        //}

        //public int DeleteLogs(LogStatus logStatus)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

    }


}
