using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Web.Models
{
    public class OKTAServiceRepository
    {
        private readonly string oktaAPIuri = ConfigurationManager.AppSettings["okta:APIuri"];
        private readonly string oktaAPIkey = ConfigurationManager.AppSettings["okta:APIkey"];
        private readonly string oktaAPPkey = ConfigurationManager.AppSettings["okta:APPkey"];
        private HttpClient Client { get; set; }

        public OKTAServiceRepository()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(oktaAPIuri.ToString() + "/v1/apps/" + oktaAPPkey.ToString() + "/users/")
            };
        }

        public UserProfile GetUserProfile(IEnumerable<System.Security.Claims.Claim> claims)
        {
            foreach (var item in claims)
            {
                if (item.Type == "sub")
                {
                    Client.DefaultRequestHeaders.Clear();
                    Client.DefaultRequestHeaders.Add("Authorization", "SSWS " + oktaAPIkey.ToString());
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = Client.GetAsync(item.Value).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var oktaUserProfile = response.Content.ReadAsStringAsync().Result;
                        var parsedObject = JObject.Parse(oktaUserProfile);
                        var credentials = parsedObject["credentials"].ToString();
                        var profile = parsedObject["profile"].ToString();
                        UserProfile userProfile = new UserProfile();
                        userProfile.userName = Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfile>(credentials).userName;
                        userProfile.email = Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfile>(profile).email;
                        userProfile.given_name = Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfile>(profile).given_name;
                        userProfile.family_name = Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfile>(profile).family_name;
                        return userProfile;
                    }
                    break;
                }
            }
            return null;
        }
    }
}
