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
                Client[] users = JsonConvert.DeserializeObject<Client[]>(File.ReadAllText(path));

                return users;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des clients");
            }
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
                    // On cherche si le role est contenu dans notre fichier
                    var searchedClient = clientsJsonFile.SingleOrDefault(item => item._id == client._id);

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

                        newArrayRoles.Add(client);
                    }
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
        }

    }
}
