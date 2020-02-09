namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Web;

    public class RolesServiceRepository
    {
        private HttpClient Client { get; set; }
        public RolesServiceRepository()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["PFP:PFPAPIuri"].ToString())
            };
        }

        public List<Roles> GetRoles()
        {
            HttpResponseMessage response = Client.GetAsync("Roles").Result;
            return response.Content.ReadAsAsync<List<Roles>>().Result;
        }

        public Roles GetRole(int id)
        {
            HttpResponseMessage response = Client.GetAsync("Roles/" + id).Result;
            return response.Content.ReadAsAsync<Roles>().Result;
        }

        public HttpResponseMessage PutRole(int id, Roles roles)
        {
            return Client.PutAsJsonAsync("Roles/" + id, roles).Result;
        }
        public HttpResponseMessage PostRole(Roles role)
        {
            return Client.PostAsJsonAsync("Roles", role).Result;
        }
        public HttpResponseMessage DeleteRole(int id)
        {
            return Client.DeleteAsync("Roles/" + id).Result;
        }

        public bool IsRoleExists(string roleName)
        {
            var roles = GetRoles();
            foreach (var role in roles)
                if (role.Name.ToLower().Replace(" ", string.Empty).Equals(roleName))
                    return true;
            return false;
        }

        public bool IsSuperAdmin(string UserEmail)
        {
            HttpResponseMessage response = Client.GetAsync("Roles/1?&useremail=" + UserEmail).Result;
            return response.Content.ReadAsAsync<bool>().Result;
        }
    }
}
