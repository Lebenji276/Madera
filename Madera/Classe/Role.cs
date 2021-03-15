using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    class Role
    {
        public string _id { get; set; }
        public string role { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public static async Task<Role> getRole(string roleId)
        {
            try
            {
                HttpResponseMessage response = await App.httpClient.GetAsync(
                    "http://localhost:5000/role/" + roleId
                );

                var role = await response.Content.ReadAsStringAsync();
                var roleJson = JsonConvert.DeserializeObject<Role>(role);

                return roleJson;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }

        }
    }
}
