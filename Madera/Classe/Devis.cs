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
    public class Devis
    {
        public string _id { get; set; }
        public string nomProjet { get; set; }
        public string client { get; set; } = null;
        public String clientString { get; set; }
        public string dateDevis { get; set; }
        public String dateDevisString { get; set; }
        public string referenceProjet { get; set; }
        public string[] modules { get; set; }
        public String modulesString { get; set; } = "";
        public PaymentData paiement { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isSynchronised { get; set; } = true;
        public bool isDeleted { get; set; } = false;

        public static Devis[] GetAllDevis()
        {
            try
            {
                var path = Json.getPath("devis");
                Devis[] devis = JsonConvert.DeserializeObject<Devis[]>(File.ReadAllText(path));
                devis = devis.Where(c => c.isDeleted == false).ToArray();

                return devis;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des devis");
            }
        }

        public static Devis GetDevisById(string id)
        {
            try
            {
                var path = Json.getPath("devis");
                Devis[] devis = JsonConvert.DeserializeObject<Devis[]>(File.ReadAllText(path));
                var singleDevis = devis.FirstOrDefault(devis => devis._id == id);

                return singleDevis;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer le devis");
            }
        }

        public static Devis CreateDevis(Devis devis)
        {
            var path = Json.getPath("devis");
            Devis[] listeDevis = JsonConvert.DeserializeObject<Devis[]>(File.ReadAllText(path));

            List<Devis> devisToRewrite = listeDevis.ToList();
            devisToRewrite.Add(devis);

            string newJson = JsonConvert.SerializeObject(devisToRewrite, Formatting.None);
            Json.writeJson("devis", newJson, true);

            return devis;
        }

        public static Devis updateDevis(Devis devis)
        {
            var path = Json.getPath("devis");
            Devis[] listDevis = JsonConvert.DeserializeObject<Devis[]>(File.ReadAllText(path));
            var dev = listDevis.FirstOrDefault(c => c._id == devis._id);
            dev = devis;

            List<Devis> devisToRewrite = new List<Devis>();
            foreach (var c in listDevis)
            {
                if (c._id != dev._id)
                {
                    devisToRewrite.Add(c);
                }
            }

            devisToRewrite.Insert(0, dev);

            string newJson = JsonConvert.SerializeObject(devisToRewrite, Formatting.None);
            Json.writeJson("devis", newJson, true);

            return devis;
        }

        public static async Task<Devis[]> GetAllDevisSynchro()
        {
            HttpResponseMessage response = await App.httpClient.GetAsync(
                "http://localhost:5000/devis/allCs"
            );
            var devis = await response.Content.ReadAsStringAsync();


            synchroDevisBase(devis);

            var rolesJson = JsonConvert.DeserializeObject<Devis[]>(devis);

            return rolesJson;
        }

        public static void synchroDevisBase(string devis)
        {
            // clients = api
            var path = Json.writeJson("devis", devis);
            List<Devis> newArrayRoles = new List<Devis>();
            Console.WriteLine(newArrayRoles);

            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                Devis[] devisJsonFile = (Devis[])serializer.Deserialize(file, typeof(Devis[]));
                Console.WriteLine(devisJsonFile);

                var devisUnserialized = JsonConvert.DeserializeObject<Devis[]>(devis);

                // On parcourt tout les roles de l'api
                foreach (var dev in devisUnserialized)
                {
                    // Client = client de l'api
                    // On cherche si le client est contenu dans notre fichier
                    var searchedDevis = devisJsonFile.SingleOrDefault(item => item._id == dev._id);
                    dev.isDeleted = searchedDevis != null && searchedDevis.isDeleted ? searchedDevis.isDeleted : false;
                    dev.isSynchronised = searchedDevis != null && searchedDevis.isSynchronised ? searchedDevis.isSynchronised : false;

                    // Si il est contenu, on regarde si il y a des différences et on le met à jour
                    if (searchedDevis != null && searchedDevis.updatedAt != dev.updatedAt)
                    {
                        if (searchedDevis.updatedAt > dev.updatedAt)
                        {
                            dev.updatedAt = searchedDevis.updatedAt;
                            dev.nomProjet = searchedDevis.nomProjet;
                            dev.modules = searchedDevis.modules;
                            dev.referenceProjet = searchedDevis.referenceProjet;
                            dev.paiement = searchedDevis.paiement;
                            dev.client = searchedDevis.client;
                            dev.dateDevis = searchedDevis.dateDevis;
                            dev.isSynchronised = false;
                        }
                    }

                    newArrayRoles.Add(dev);
                }

                // On synchro les modifs locales également
                foreach (var dev in devisJsonFile)
                {
                    if (devisUnserialized.SingleOrDefault(item => item._id == dev._id) == null)
                    {
                        dev.isSynchronised = false;
                        newArrayRoles.Add(dev);
                    }
                }
            }

            string newJson = JsonConvert.SerializeObject(newArrayRoles, Formatting.None);
            Json.writeJson("devis", newJson, true);
        }

        public static async Task<bool> synchroDevisToAPI()
        {
            // Recup des objets du json
            var path = Json.getPath("devis");
            Devis[] devis = JsonConvert.DeserializeObject<Devis[]>(File.ReadAllText(path));
            List<Devis> devisToRewriteJson = new List<Devis>();

            // On récupère les objets qui ne sont pas synchro
            var devisToSend = devis.Where(dev => dev.isSynchronised == false);

            if (devisToSend == null)
            {
                return true;
            }

            foreach (var dev in devisToSend)
            {
                // On convertit en JSON
                var devisToSendJSON = JsonConvert.SerializeObject(dev, Formatting.None);

                // On le delete si il a été delete
                if (dev.isDeleted)
                {
                    //var delete = await App.httpClient.DeleteAsync("http://localhost:5000/devis/" + dev._id);

                    //if (!delete.IsSuccessStatusCode)
                    //{
                    //    devisToRewriteJson.Add(dev);
                    //}
                }
                else
                {
                    // On essaye de récup par client id voir si il existe
                    var get = await App.httpClient.GetAsync("http://localhost:5000/devis/" + dev._id);
                    HttpResponseMessage create;
                    string responseString = await get.Content.ReadAsStringAsync();

                    dev.modulesString = JsonConvert.SerializeObject(dev.modules, Formatting.None);

                    var values = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("nomProjet", dev.nomProjet),
                        new KeyValuePair<string, string>("client", dev.client),
                        new KeyValuePair<string, string>("dateDevis", dev.dateDevis),
                        new KeyValuePair<string, string>("referenceProjet", dev.referenceProjet),
                        new KeyValuePair<string, string>("modules", dev.modulesString),
                    };

                    if (get.IsSuccessStatusCode && !String.IsNullOrEmpty(responseString))
                    {
                        create = await App.httpClient.PutAsync(
                                        "http://localhost:5000/devis/" + dev._id,
                                        new FormUrlEncodedContent(values)
                                        );

                        if (create.IsSuccessStatusCode)
                        {
                            dev.isSynchronised = true;
                        }
                        // update
                    }
                    else
                    {
                        create = await App.httpClient.PostAsync(
                                        "http://localhost:5000/devis",
                                        new FormUrlEncodedContent(values)
                                        );

                        if (create.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            var content = await create.Content.ReadAsStringAsync();
                            var devisToApi = JsonConvert.DeserializeObject<Devis>(content);
                            dev._id = devisToApi._id;
                            dev.isSynchronised = true;
                        }
                    }

                    devisToRewriteJson.Add(dev);
                }
            }

            foreach (var dev in devis.Where(role => role.isSynchronised != false))
            {
                // On ajoute que si pas présent
                if (devisToRewriteJson.FirstOrDefault(r => dev._id == r._id) == null)
                {
                    devisToRewriteJson.Add(dev);
                }
            }

            string newJson = JsonConvert.SerializeObject(devisToRewriteJson, Formatting.None);
            Json.writeJson("devis", newJson, true);

            return true;
        }
    }

    
}
