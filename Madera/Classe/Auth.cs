using DevOne.Security.Cryptography.BCrypt;
using Madera.Jsons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Madera.Classe
{
    class Auth
    {
        public string access_token { get; set; }
        public string role { get; set; }
        public Role roleDescription { get; set; }

        public static bool checkAuth(string username, string password)
        {
            try
            {
                var path = Json.getPath("user");

                if (String.IsNullOrEmpty(path))
                {
                    throw new Exception("Veuillez vous connecter via internet");
                }

                User user = JsonConvert.DeserializeObject<User>(File.ReadAllText(path));

                if (user != null && user.username == username)
                {
                    var checkedPassword = BCryptHelper.CheckPassword(password, user.password);

                    return checkedPassword ? true : false;
                }
                else
                {
                    throw new Exception("Impossible de connecter l'utilisateur");
                }
            } catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<Auth> postAuth(string username, string password)
        {
            try
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password",password) 
                };

                HttpResponseMessage response = await App.httpClient.PostAsync(
                    "http://localhost:5000/auth/login",
                    new FormUrlEncodedContent(values)
                );

                if (response.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    throw new Exception("Impossible de vous connecter avec ces identifiants");
                }

                var authentication = await response.Content.ReadAsStringAsync();
                var authJson = JsonConvert.DeserializeObject<Auth>(authentication);

                App.setBearer(authJson.access_token);

                authJson.roleDescription = await Role.getRole(authJson.role);

                return authJson;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }
    }
}
