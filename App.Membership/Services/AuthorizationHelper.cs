using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using App.Membership.Domain;
using App.Membership.Services.Logging;
using App.Membership.Services.Logging.Enums;

namespace App.Membership.Services
{
    public static class AuthorizationHelper
    {
        public static bool HasAccess(this IIdentity user, string rightName)
        {

            return HasAccess(user.Name, rightName);
        }

        public static bool HasAccess(this IIdentity user)
        {
            MethodBase method = (new StackTrace()).GetFrame(1).GetMethod();
            string rightName = method.DeclaringType.Name + "." + method.Name;
            return HasAccess(user.Name, rightName);
        }

        public static bool HasAccess(string userName, string rightName, bool logged = false)
        {
            bool hasRight;
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(rightName))
                {
                    throw new ArgumentNullException("İstifadəçi adı və/və ya metod adı daxil edilməyib!");
                }

                var rightRep = Repositories.RepositoryFactory.GetRightRepository();

                var resultRight = rightRep.CheckUserHasRight(userName, rightName);
                if (!resultRight.Success)
                    throw new Exception("Xəta", resultRight.Errors[0]);

                hasRight = resultRight.Value;

                if (logged)
                {
                    var logData = new LogData()
                    {
                        ActionId = 3,    // HasAccess Action
                        Level = Level.Info,
                        RightName = rightName,
                        LogActionResult = hasRight ? LogActionResult.Success : LogActionResult.Failed,
                        EventId = hasRight ? 8 : 9,
                    };
                    LoggingService.Current.WriteLog(logData);
                }


            }
            catch (Exception ex)
            {
                hasRight = false;
            }

            return hasRight;
        }

        public static bool HasAccess(string userName, string rightName, ref List<Right> rights, bool logged = false)
        {
            bool hasRight = false;
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(rightName))
                {
                    throw new ArgumentNullException("İstifadəçi adı və/və ya metod adı daxil edilməyib!");
                }

                if (rights.Count > 0)
                {
                    hasRight = rights.Any(x => x.Name == rightName);
                }
                else
                {
                    var rightRep = Repositories.RepositoryFactory.GetRightRepository();

                    var checkUserHasRight = rightRep.CheckUserHasRight(userName, rightName);
                    if (!checkUserHasRight.Success)
                        throw checkUserHasRight.Errors[0];
                    hasRight = checkUserHasRight.Value;
                }


                if (logged)
                {
                    var logData = new LogData()
                    {
                        ActionId = 3,    // HasAccess Action
                        Level = Level.Info,
                        RightName = rightName,
                        LogActionResult = hasRight ? LogActionResult.Success : LogActionResult.Failed,
                        EventId = hasRight ? 8 : 9,
                    };
                    LoggingService.Current.WriteLog(logData);
                }


            }
            catch (Exception ex)
            {
                hasRight = false;
            }

            return hasRight;
        }




        /*/// <summary>
        /// Bu Stack Trace-də əvvəlki metodun adını qaytarır,yani indexi 1 olan
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static bool HasAccess(this IIdentity user) 
        {
            string  callingMethod = new System.Diagnostics.StackTrace(1, false).GetFrame(0).GetMethod().Name;
            return  HasAccess(user.Name, callingMethod);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static bool HasAccess(string userName) 
        {
            string callingMethod = new System.Diagnostics.StackTrace(1, false).GetFrame(0).GetMethod().Name;
            return   HasAccess(userName, callingMethod);
        }
        */


    }
}
