using System;
using System.Security.Cryptography;
using System.Text;
using App.Membership.Repositories;
using App.Membership.Repositories.Abstract;
using System.Web.Security;
using App.Membership.Domain;
using App.Membership.Services.Logging;
using App.Membership.Services.Logging.Enums;

namespace App.Membership.Providers
{
    public class AgMembershipProvider : MembershipProvider
    {

        private IUserRepository _repository;

        public AgMembershipProvider()
            : this(RepositoryFactory.GetUserRepository())
        {
        }

        public AgMembershipProvider(IUserRepository repository)
            : base()
        {
            this._repository = repository;
        }

        private int _minRequiredPasswordLength = 6;
        private bool _requiresUniqueEmail = false;

        public bool ResetPasswordByAdmin(string username, string newPassword)
        {
            var foundUser = _repository.FindBy(username);
            if (!foundUser.Success || foundUser.Value == null)
            {
                return false;
            }
            foundUser.Value.Password = HashPassword(newPassword);
            _repository.Update(foundUser.Value);
            return true;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool result;

            LogData logData = new LogData()
            {
                ActionId = 2, //ChangePassword    
                Description = null,
                Level = Level.Info
            };

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(args);


            if (!ValidateUser(username, oldPassword))
            {
                logData.LogActionResult = LogActionResult.Failed;
                logData.EventId = 3; // Changing password failed. Wrong old password.
                result = false;
            }
            else if (string.IsNullOrEmpty(newPassword))
            {
                logData.LogActionResult = LogActionResult.Failed;
                logData.EventId = 4; // Changing password failed. Invalid new password.
                result = false;
            }
            else
            {
                if (!_repository.ChangePassword(username, HashPassword(oldPassword), HashPassword(newPassword)).Success)
                {
                    logData.LogActionResult = LogActionResult.Failed;
                    logData.EventId = 5; //Changing password failed. Unexpected error.
                    result = false;
                }
                else
                {
                    logData.LogActionResult = LogActionResult.Success;
                    logData.EventId = 6; //Password was changed successfully.
                    result = true;
                }
            }
            LoggingService.Current.WriteLog(logData);
            return result;

        }

        private string HashPassword(string password)
        {
            if (password == null)
            {
                return null;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(password);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);

        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        /*private User MapToUser(MembershipUser membershipUser)
        {
            if (membershipUser == null) { return null; }
            User user = new User()
            {

            };
              throw new NotImplementedException(); 
        }*/

        private MembershipUser MapToMembershipUser(User user)
        {

            return new MembershipUser("AgMembershipProvider", user.Username, user.Username, string.Empty,
                string.Empty, string.Empty, true, !user.IsEnabled, DateTime.Now,
                DateTime.MinValue, DateTime.MinValue, DateTime.MinValue,
                DateTime.MinValue
            );

        }

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey,
            out MembershipCreateStatus status)
        {
            username = username.ToLower();
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            /* not used yet
             * if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
             {
                 status = MembershipCreateStatus.DuplicateEmail;
                 return null;
             }*/

            MembershipUser user = GetUser(username, true);

            if (user == null)
            {


                user = new MembershipUser("AgMembershipProvider", username, providerUserKey, email, passwordQuestion,
                    null, isApproved, false, DateTime.Now,
                    DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);


                User newUser = new User()
                {
                    Username = username,
                    Password = HashPassword(password),
                    IsDeleted = false,
                    IsEnabled = true
                };


                _repository.Add(newUser);

                status = MembershipCreateStatus.Success;
                return GetUser(username, true);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }


        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            _repository.Delete(username);
            return true;

        }

        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }


        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var domainUsers = _repository.GetAllUsers(pageIndex, pageSize, out totalRecords);
            var users = new MembershipUserCollection();
            if (domainUsers.TotalItems <= 0) return users;
            foreach (var user in domainUsers.List)
            {
                users.Add(MapToMembershipUser(user));
            }
            return users;
        }


        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var foundUser = _repository.FindBy(username);

            if (!foundUser.Success || foundUser.Value == null)
            {
                return null;
            }
            return MapToMembershipUser(foundUser.Value);

        }




        public override bool UnlockUser(string userName)
        {

            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {

            throw new NotImplementedException();
        }

        public void ChangeUserEnabledStatus(string username, bool isEnabled, string description = "", int eventId = -1)
        {
            LogData logData = new LogData()
            {
                ActionId = 6,
                Level = Level.Info
            };

            var foundUser = _repository.FindBy(username);
            if (foundUser.Value == null)
            {
                return;
            }
            if (foundUser.Value.IsEnabled != isEnabled)
            {
                foundUser.Value.IsEnabled = isEnabled;
                if (!isEnabled)
                {
                    description = (string.IsNullOrWhiteSpace(description)) ? "Disabled" : description;
                    logData.EventId = (eventId == -1 ? 2 : eventId); //Disabled
                    foundUser.Value.DisabledDate = DateTime.Now;
                }
                else
                {
                    description = (string.IsNullOrWhiteSpace(description)) ? "Enabled" : description;
                    logData.EventId = (eventId == -1 ? 1 : eventId); //Enabled
                }
                _repository.Update(foundUser.Value);
                logData.Description = description;
                logData.LogActionResult = LogActionResult.Success;
                LoggingService.Current.WriteLog(logData);
            }
        }


        public override bool ValidateUser(string username, string password)
        {
            try
            {
                var resultHasValid = _repository.ValidateUser(username, HashPassword(password));
                if (!resultHasValid.Success) return false;
                return resultHasValid.Value;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey != null)
                return GetUser((string)providerUserKey, userIsOnline);
            else
                return null;
        }

        //    ///////////////

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }


        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Default 6
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
            //set { minRequiredPasswordLength = value; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Default false
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
            //set { requiresUniqueEmail = value; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {

            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }



        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        #region not used

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
