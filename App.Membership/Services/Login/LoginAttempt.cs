using System;

namespace App.Membership.Services.Login
{
    
   public  class LoginAttempt
    {
       private static int _idSequence;

       private int _id;
       public int Id 
       { 
           get 
           {
               if (_id == 0)
               {
                   _id = ++_idSequence;
                   return _id;
               }
               else 
               {
                   return _id;
               }
           }
       }
       public string Ip { get; set; }
       public string UserName { get; set; }
       public DateTime AttemptDate { get; set; }
       public int FailedAttemptCount { get; set; }
       public LoginAttempt(string ip, string userName) 
       {
           Ip = ip;
           UserName = userName;
           AttemptDate = DateTime.Now;
       }
       public override bool Equals(object obj)
       {
           var loginAttempt = (LoginAttempt)obj;
           return loginAttempt != null && Ip == loginAttempt.Ip && UserName == loginAttempt.UserName;
       }
       public override int GetHashCode()
       {
           return  Id;
       }

       //public bool Equals(LoginAttempt x, LoginAttempt y)
       //{
       //    return x.IP == y.IP && x.UserName == y.UserName;
       //}

       //public int GetHashCode(LoginAttempt obj)
       //{
       //    return obj.IP + obj.UserName;
       //}
    }
}
