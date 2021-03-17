using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    class Composant
    {
        public string _id { get; set; }
        public string nomComposant { get; set; }
        public string unité { get; set; }
        public string nomCaracteristique { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public string description { get; set; }
        public override string ToString()
        {
            return this.nomComposant;
        }

        public static async Task<Composant[]> GetAllComposant()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:5000/composant").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    var listClient = JsonConvert.DeserializeObject<Composant[]>(responseString);
                    return listClient;
                }
            }
            return null;
        }

        public static async Task<Composant> CreateComposant(Composant composant)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("nomComposant", composant.nomComposant),
                    new KeyValuePair<string, string>("description",composant.description)
                };

                HttpResponseMessage response = await client.PostAsync(
                    "http://localhost:5000/composant",
                    new FormUrlEncodedContent(values)
                );


                var appointmentResponse = await response.Content.ReadAsStringAsync();
                var appointmentJson = JsonConvert.DeserializeObject<Composant>(appointmentResponse);


                return appointmentJson;
            }

        }

    }
}
