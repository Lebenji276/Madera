using Madera.Jsons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Madera
{
    public class Client
    {
        public string _id { get; set; }
        public string first_name { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isSynchronised { get; set; } = true;
        public bool isDeleted { get; set; } = false;

        public override string ToString()
        {
            return this.first_name;
        }

        public static Client[] GetAllClient()
        {
            try
            {
                var path = Json.getPath("clients");
                Client[] clients = JsonConvert.DeserializeObject<Client[]>(File.ReadAllText(path));
                clients = clients.Where(c => c.isDeleted == false).ToArray();

                return clients;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des clients");
            }
        }

        public static Client GetClientById(string id)
        {
            try
            {
                var path = Json.getPath("clients");
                Client[] clients = JsonConvert.DeserializeObject<Client[]>(File.ReadAllText(path));
                var client = clients.FirstOrDefault(client => client._id == id);

                return client;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des clients");
            }
        }

        public static Client updateClient(Client client)
        {
            var path = Json.getPath("clients");
            Client[] clients = JsonConvert.DeserializeObject<Client[]>(File.ReadAllText(path));
            var cli = clients.FirstOrDefault(c => c._id == client._id);
            cli = client;

            List<Client> clientsToRewrite = new List<Client>();
            foreach(var c in clients)
            {
                if(c._id != cli._id)
                {
                    clientsToRewrite.Add(c);
                }
            }

            clientsToRewrite.Insert(0, cli);

            string newJson = JsonConvert.SerializeObject(clientsToRewrite, Formatting.None);
            Json.writeJson("clients", newJson, true);

            return client;
        }

        public static async Task<Client[]> GetAllClientSynchro()
        {
            HttpResponseMessage response = await App.httpClient.GetAsync(
                "http://localhost:5000/client"
            );
            var clients = await response.Content.ReadAsStringAsync();


            synchroClientsBase(clients);

            var rolesJson = JsonConvert.DeserializeObject<Client[]>(clients);

            return rolesJson;
        }

        public static void synchroClientsBase(string clients)
        {
            // clients = api
            var path = Json.writeJson("clients", clients);
            List<Client> newArrayRoles = new List<Client>();

            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                Client[] clientsJsonFile = (Client[])serializer.Deserialize(file, typeof(Client[]));

                var clientsUnserialized = JsonConvert.DeserializeObject<Client[]>(clients);

                // On parcourt tout les roles de l'api
                foreach (var client in clientsUnserialized)
                {
                    // Client = client de l'api
                    // On cherche si le client est contenu dans notre fichier
                    var searchedClient = clientsJsonFile.SingleOrDefault(item => item._id == client._id);
                    client.isDeleted = searchedClient.isDeleted ? searchedClient.isDeleted : false;
                    client.isSynchronised = searchedClient.isSynchronised ? searchedClient.isSynchronised : false;

                    // Si il est contenu, on regarde si il y a des différences et on le met à jour
                    if (searchedClient != null && searchedClient.updatedAt != client.updatedAt)
                    {
                        if (searchedClient.updatedAt > client.updatedAt)
                        {
                            client.updatedAt = searchedClient.updatedAt;
                            client.address = searchedClient.address;
                            client.mail = searchedClient.mail;
                            client.phone = searchedClient.phone;
                            client.first_name = searchedClient.first_name;
                            client.postal_code = searchedClient.postal_code;
                            client.city = searchedClient.city;
                            client.country = searchedClient.country;
                            client.isSynchronised = false;
                        }
                    }

                    newArrayRoles.Add(client);
                }

                // On synchro les modifs locales également
                foreach (var client in clientsJsonFile)
                {
                    if (clientsUnserialized.SingleOrDefault(item => item._id == client._id) == null)
                    {
                        client.isSynchronised = false;
                        newArrayRoles.Add(client);
                    }
                }
            }

            string newJson = JsonConvert.SerializeObject(newArrayRoles, Formatting.None);
            Json.writeJson("clients", newJson, true);
        }


        public static async Task<bool> synchroClientsToAPI()
        {
            // Recup des objets du json
            var path = Json.getPath("clients");
            Client[] clients = JsonConvert.DeserializeObject<Client[]>(File.ReadAllText(path));
            List<Client> clientsToRewriteJson = new List<Client>();

            // On récupère les objets qui ne sont pas synchro
            var clientsToSend = clients.Where(client => client.isSynchronised == false);

            if (clientsToSend == null)
            {
                return true;
            }

            foreach (var client in clientsToSend)
            {
                // On convertit en JSON
                var clientToSendJSON = JsonConvert.SerializeObject(client, Formatting.None);

                // On le delete si il a été delete
                if (client.isDeleted)
                {
                    var delete = await App.httpClient.DeleteAsync("http://localhost:5000/client/" + client._id);

                    if (!delete.IsSuccessStatusCode)
                    {
                        clientsToRewriteJson.Add(client);
                    }
                }
                else
                {
                    // On essaye de récup par client id voir si il existe
                    var get = await App.httpClient.GetAsync("http://localhost:5000/client/" + client._id);
                    HttpResponseMessage create;

                    var values = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("first_name", client.first_name),
                        new KeyValuePair<string, string>("mail", client.mail),
                        new KeyValuePair<string, string>("phone", client.phone),
                        new KeyValuePair<string, string>("address", client.address),
                        new KeyValuePair<string, string>("postal_code", client.postal_code),
                        new KeyValuePair<string, string>("city", client.city),
                        new KeyValuePair<string, string>("country", client.country),
                    };

                    if (get.IsSuccessStatusCode)
                    {
                        create = await App.httpClient.PutAsync(
                                        "http://localhost:5000/client/" + client._id,
                                        new FormUrlEncodedContent(values)
                                        );

                        if (create.IsSuccessStatusCode)
                        {
                            client.isSynchronised = true;
                        }
                        // update
                    }
                    else
                    {
                        create = await App.httpClient.PostAsync(
                                        "http://localhost:5000/client/create",
                                        new FormUrlEncodedContent(values)
                                        );

                        if (create.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            client.isSynchronised = true;
                        }
                    }

                    clientsToRewriteJson.Add(client);
                }
            }

            foreach (var client in clients.Where(role => role.isSynchronised != false))
            {
                // On ajoute que si pas présent
                if (clientsToRewriteJson.FirstOrDefault(r => client._id == r._id) == null)
                {
                    clientsToRewriteJson.Add(client);
                }
            }

            string newJson = JsonConvert.SerializeObject(clientsToRewriteJson, Formatting.None);
            Json.writeJson("clients", newJson, true);

            return true;
        }

    }
}
