using Madera.Jsons;
using MongoDB.Bson;
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
    public class Module
    {
        public string _id { get; set; }
        public Composant[] composants { get; set; }
        public string nomModule { get; set; }
        public string nomGamme { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string description { get; set; }
        public bool isSynchronised { get; set; } = true;
        public bool isDeleted { get; set; } = false;
        public string composant { get; set; }
        public string composantsString
        {

            get => listComposantToString(composants);
        }

        public override string ToString()
        {
            return this.nomModule;
        }

        public static Module[] GetAllModule()
        {
            try
            {
                var path = Json.getPath("modules");
                Module[] modules = JsonConvert.DeserializeObject<Module[]>(File.ReadAllText(path));
                modules = modules.Where(c => c.isDeleted == false).ToArray();

                return modules;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des modules");
            }
        }

        private string listComposantToString(Composant[] composants)
        {
            string result = "";
            if (composants != null)
            {
                foreach (var item in composants)
                {
                    result += item.ToString() + ", ";
                }
            }
            return result != "" ? result.Substring(0, result.Length - 2) : result;
        }

        public static Module CreateModule(Module client)
        {
            List<Module> modulesToRewrite = new List<Module>();
            var path = Json.getPath("modules");
            Module[] modules = JsonConvert.DeserializeObject<Module[]>(File.ReadAllText(path));

            client._id = ObjectId.GenerateNewId().ToString();
            foreach (var c in modules)
            {
                if (c._id != client._id)
                {
                    modulesToRewrite.Add(c);
                }
            }
            modulesToRewrite.Insert(0, client);

            string newJson = JsonConvert.SerializeObject(modulesToRewrite, Formatting.None);
            Json.writeJson("modules", newJson, true);

            return client;
        }

        public static Module UpdateModule(Module client)
        {
            List<Module> modulesToRewrite = new List<Module>();
            var path = Json.getPath("modules");
            Module[] modules = JsonConvert.DeserializeObject<Module[]>(File.ReadAllText(path));

            foreach (var c in modules)
            {
                if (c._id != client._id)
                {
                    modulesToRewrite.Add(c);
                }
            }
            modulesToRewrite.Insert(0, client);

            string newJson = JsonConvert.SerializeObject(modulesToRewrite, Formatting.None);
            Json.writeJson("modules", newJson, true);

            return client;
        }

        public static async Task<Module[]> GetAllModuleSynchro()
        {
            HttpResponseMessage response = await App.httpClient.GetAsync(
                "http://localhost:5000/module/all"
            );
            var modules = await response.Content.ReadAsStringAsync();


            synchroModulesBase(modules);

            var rolesJson = JsonConvert.DeserializeObject<Module[]>(modules);

            return rolesJson;
        }

        public static void synchroModulesBase(string modules)
        {
            // modules = api
            var path = Json.writeJson("modules", modules);
            List<Module> newArrayRoles = new List<Module>();

            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                Module[] modulesJsonFile = (Module[])serializer.Deserialize(file, typeof(Module[]));

                var modulesUnserialized = JsonConvert.DeserializeObject<Module[]>(modules);

                // On parcourt tout les roles de l'api
                foreach (var module in modulesUnserialized)
                {
                    var searchedModule = modulesJsonFile.SingleOrDefault(item => item._id == module._id);
                    module.isDeleted = searchedModule.isDeleted ? searchedModule.isDeleted : false;
                    module.isSynchronised = searchedModule.isSynchronised ? searchedModule.isSynchronised : false;

                    // Si il est contenu, on regarde si il y a des différences et on le met à jour
                    if (searchedModule != null && searchedModule.updatedAt != module.updatedAt)
                    {
                        if (searchedModule.updatedAt > module.updatedAt)
                        {
                            module.updatedAt = searchedModule.updatedAt;
                            module.nomModule = searchedModule.nomModule;
                            module.nomGamme = searchedModule.nomGamme;
                            module.description = searchedModule.description;
                            module.composants = searchedModule.composants;
                            module.isSynchronised = false;
                        }
                    }

                    newArrayRoles.Add(module);
                }

                // On synchro les modifs locales également
                foreach (var module in modulesJsonFile)
                {
                    if (modulesUnserialized.SingleOrDefault(item => item._id == module._id) == null)
                    {
                        module.isSynchronised = false;
                        newArrayRoles.Add(module);
                    }
                }
            }

            string newJson = JsonConvert.SerializeObject(newArrayRoles, Formatting.None);
            Json.writeJson("modules", newJson, true);
        }

        public static async Task<bool> synchroModulesToAPI()
        {
            // Recup des objets du json
            var path = Json.getPath("modules");
            Module[] clients = JsonConvert.DeserializeObject<Module[]>(File.ReadAllText(path));
            List<Module> clientsToRewriteJson = new List<Module>();

            // On récupère les objets qui ne sont pas synchro
            var clientsToSend = clients.Where(client => client.isSynchronised == false);

            if (clientsToSend == null)
            {
                return true;
            }

            foreach (var module in clientsToSend)
            {
                // On convertit en JSON
                var clientToSendJSON = JsonConvert.SerializeObject(module, Formatting.None);

                // On le delete si il a été delete
                if (module.isDeleted)
                {
                    var delete = await App.httpClient.DeleteAsync("http://localhost:5000/module/" + module._id);

                    if (!delete.IsSuccessStatusCode)
                    {
                        clientsToRewriteJson.Add(module);
                    }
                }
                else
                {
                    // On essaye de récup par client id voir si il existe
                    var get = await App.httpClient.GetAsync("http://localhost:5000/module/" + module._id);
                    HttpResponseMessage create;

                    var values = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("composants", JsonConvert.SerializeObject(module.composants, Formatting.None)),
                        new KeyValuePair<string, string>("description", module.description),
                        new KeyValuePair<string, string>("nomGamme", module.nomGamme),
                        new KeyValuePair<string, string>("nomModule", module.nomModule),    
                    };

                    if (get.IsSuccessStatusCode)
                    {
                        create = await App.httpClient.PutAsync(
                                        "http://localhost:5000/module/composants/" + module._id,
                                        new FormUrlEncodedContent(values)
                                        );

                        if (create.IsSuccessStatusCode)
                        {
                            module.isSynchronised = true;
                        }
                        // update
                    }
                    else
                    {
                        create = await App.httpClient.PostAsync(
                                        "http://localhost:5000/module",
                                        new FormUrlEncodedContent(values)
                                        );

                        if (create.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            module.isSynchronised = true;
                        }
                    }

                    clientsToRewriteJson.Add(module);
                }
            }

            foreach (var module in clients.Where(role => role.isSynchronised != false))
            {
                // On ajoute que si pas présent
                if (clientsToRewriteJson.FirstOrDefault(r => module._id == r._id) == null)
                {
                    clientsToRewriteJson.Add(module);
                }
            }

            string newJson = JsonConvert.SerializeObject(clientsToRewriteJson, Formatting.None);
            Json.writeJson("modules", newJson, true);

            return true;
        }
    }
}
