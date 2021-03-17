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
    class Role
    {
        public string _id { get; set; }
        public string role { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isSynchronised { get; set; } = true;
        public bool isDeleted { get; set; } = false;

        public static void synchroRolesBase(string roles)
        {
            var path = Json.writeJson("roles", roles);
            List<Role> newArrayRoles = new List<Role>();

            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                Role[] rolesJsonFile = (Role[])serializer.Deserialize(file, typeof(Role[]));

                var rolesUnserialized = JsonConvert.DeserializeObject<Role[]>(roles);

                // On parcourt tout les roles de l'api
                foreach (var role in rolesUnserialized)
                {
                    // On cherche si le role est contenu dans notre fichier
                    var searchedRole = rolesJsonFile.SingleOrDefault(item => item._id == role._id);
                    role.isDeleted = searchedRole.isDeleted;
                    role.isSynchronised = searchedRole.isSynchronised;

                    // Si il est contenu, on regarde si il y a des différences et on le met à jour
                    if (searchedRole != null && searchedRole.updatedAt != role.updatedAt)
                    {
                        if (searchedRole.updatedAt > role.updatedAt)
                        {
                            role.updatedAt = searchedRole.updatedAt;
                            role.role = searchedRole.role;
                            role.isSynchronised = false;
                        }
                    }

                    newArrayRoles.Add(role);
                }

                // On synchro les modifs locales également
                foreach (var role in rolesJsonFile)
                {
                    if (rolesUnserialized.SingleOrDefault(item => item._id == role._id) == null)
                    {
                        role.isSynchronised = false;
                        newArrayRoles.Add(role);
                    }
                }
            }

            string newJson = JsonConvert.SerializeObject(newArrayRoles, Formatting.None);
            Json.writeJson("roles", newJson, true);
        }

        public static async Task<Role[]> getAllRoles(bool isSynchro = false)
        {
            HttpResponseMessage response = await App.httpClient.GetAsync(
                "http://localhost:5000/role"
            );
            var roles = await response.Content.ReadAsStringAsync();

            if (isSynchro)
            {
                synchroRolesBase(roles);
            }

            var rolesJson = JsonConvert.DeserializeObject<Role[]>(roles);

            return rolesJson;
        }

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

        public static async Task<bool> synchroRoleAPI()
        {
            // Recup des objets du json
            var path = Json.getPath("roles");
            Role[] roles = JsonConvert.DeserializeObject<Role[]>(File.ReadAllText(path));
            List<Role> rolesToRewriteJson = new List<Role>();

            // On récupère les objets qui ne sont pas synchro
            var rolesToSend = roles.Where(role => role.isSynchronised == false);

            if (rolesToSend == null)
            {
                return true;
            }

            foreach(var role in rolesToSend)
            {
                // On convertit en JSON
                var roleToSendJSON = JsonConvert.SerializeObject(role, Formatting.None);

                // On le delete si il a été delete
                if (role.isDeleted)
                {
                    var delete = await App.httpClient.DeleteAsync("http://localhost:5000/role/"+role._id);

                    if (! delete.IsSuccessStatusCode)
                    {
                        rolesToRewriteJson.Add(role);
                    }
                } else
                {
                    // On essaye de récup par role id voir si il existe
                    var get = await App.httpClient.GetAsync("http://localhost:5000/role/"+role._id);
                    HttpResponseMessage create;

                    var values = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("role", role.role)
                    };

                    if (get.IsSuccessStatusCode)
                    {
                        create = await App.httpClient.PatchAsync(
                                        "http://localhost:5000/role/" + role._id,
                                        new FormUrlEncodedContent(values)
                                        );
                        Console.WriteLine(create);

                        if (create.IsSuccessStatusCode)
                        {
                            role.isSynchronised = true;
                        }
                        // update
                    } else
                    {
                        create = await App.httpClient.PostAsync(
                                        "http://localhost:5000/role",
                                        new FormUrlEncodedContent(values)
                                        );

                        if (create.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            role.isSynchronised = true;
                        }
                    }

                    

                    rolesToRewriteJson.Add(role);
                }
            }

            foreach (var role in roles.Where(role => role.isSynchronised != false))
            {
                // On ajoute que si pas présent
                if (rolesToRewriteJson.FirstOrDefault(r => role._id == r._id) == null)
                {
                    rolesToRewriteJson.Add(role);
                }
            }

            string newJson = JsonConvert.SerializeObject(rolesToRewriteJson, Formatting.None);
            Json.writeJson("roles", newJson, true);

            return true;
        }
    }
}
