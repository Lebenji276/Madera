using Madera.Jsons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    class User
    {
        public string _id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string password { get; set; }
        public Role role { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isSynchronised { get; set; } = true;
        public bool isDeleted { get; set; } = false;

        public static void synchroUsersBase(string users)
        {
            var path = Json.writeJson("users", users);
            List<User> newArrayUsers = new List<User>();

            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                User[] usersJsonFile = (User[])serializer.Deserialize(file, typeof(User[]));

                var usersUnserialized = JsonConvert.DeserializeObject<User[]>(users);

                // On parcourt tout les users de l'api
                foreach (var user in usersUnserialized)
                {
                    newArrayUsers.Add(user);
                }
            }

            string newJson = JsonConvert.SerializeObject(newArrayUsers, Formatting.None);
            Json.writeJson("users", newJson, true);
        }

        public static async Task<User[]> getAllUsers(bool isSynchro = false)
        {
            HttpResponseMessage response = await App.httpClient.GetAsync(
                "http://localhost:5000/user/all"
            );
            var users = await response.Content.ReadAsStringAsync();

            if (isSynchro)
            {
                synchroUsersBase(users);
            }

            var rolesJson = JsonConvert.DeserializeObject<User[]>(users);

            return rolesJson;
        }

        public static User getUser(string username)
        {
            var path = Json.getPath("users");
            User[] users = JsonConvert.DeserializeObject<User[]>(File.ReadAllText(path));

            var user = users.FirstOrDefault(user => user.username == username);

            if (user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("Impossible de connecter l'utilisateur");
            }  
        }
    }
}
