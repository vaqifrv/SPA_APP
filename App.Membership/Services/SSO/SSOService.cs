using System;
using System.Net.Http;
using System.Text;
using App.Membership.Services.SSO.Configuration;
using Newtonsoft.Json;

namespace App.Membership.Services.SSO
{
    public class SsoService
    {
        private string SsoServiceUri
        {
            get
            {
                object section = System.Configuration.ConfigurationManager.GetSection("SingleSignOn");
                if (section != null)
                    return ((SingleSignOnSection)section).Settings.ServiceUri;
                else
                    return "";
            }
        }
        private string SsoServer
        {
            get
            {
                object section = System.Configuration.ConfigurationManager.GetSection("SingleSignOn");
                if (section != null)
                    return ((SingleSignOnSection)section).Settings.SsoServer;
                else
                    return "";
            }
        }

        public string CreateToken(SecurityToken token)
        {
            string id = "";
            HttpClient client = null;
            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(SsoServiceUri);
                client.DefaultRequestHeaders.Clear();

                HttpContent content = new ByteArrayContent(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(token)));                
                HttpResponseMessage response = client.PutAsync("api/security/sso", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    id = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result).ToString();
                }

            }
            finally
            {
                if (client != null)
                    client.Dispose();
            }

            return id;
        }


        public SecurityToken GetToken(string id)
        {
            SecurityToken token = null;
            HttpClient client = null;
            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(SsoServiceUri);
                client.DefaultRequestHeaders.Clear();
                
                HttpResponseMessage response = client.GetAsync("api/security/sso/"+id).Result;
                if (response.IsSuccessStatusCode)
                {
                    token = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SecurityToken)) as SecurityToken;
                }
            }
            finally
            {
                if (client != null)
                    client.Dispose();
            }

            return token;
        }


        public SecurityToken GetLoginBySso(string returnUrl, string token, bool sso)
        {
            SecurityToken result = new SecurityToken();
            if (!sso)
            {
                if (!string.IsNullOrEmpty(token))
                {
                    SecurityToken secToken = GetToken(token);
                    result.Username = secToken.Username;
                    result.ReturnUrl = secToken.ReturnUrl;
                }
                else if (!String.IsNullOrEmpty(SsoServer))
                {
                    token = CreateToken(new SecurityToken { Username = "", ReturnUrl = returnUrl });
                    if (!string.IsNullOrEmpty(token))
                    {
                        result.ReturnUrl = SsoServer + "Security/Login/SSOLogin?token=" + token;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(token))
            {
                SecurityToken secToken = GetToken(token);
                if (secToken != null && !string.IsNullOrEmpty(secToken.ReturnUrl))
                    result.ReturnUrl = secToken.ReturnUrl;
            }

            return result;

        }

        public string PostLoginToSso(string returnUrl, string userName)
        {
            if (String.IsNullOrEmpty(SsoServer))
                return returnUrl;
            else
            {
                string token = CreateToken(new SecurityToken { Username = userName, ReturnUrl = returnUrl });

                if (String.IsNullOrEmpty(token))
                    return returnUrl;
                else
                {
                    return SsoServer + "Security/Login/SSOLogin?token=" + token;
                }
            }

        }

        public string LogOffFromSso(string returnUrl)
        {
            if (!String.IsNullOrEmpty(SsoServer))
            {
                SsoService service = new SsoService();
                string token = service.CreateToken(new SecurityToken { ReturnUrl = returnUrl });
                if (!string.IsNullOrEmpty(token))
                    return SsoServer + "Security/Login/SSOLogOff?token=" + token;
            }
            
            return returnUrl;
        }
    }
}
