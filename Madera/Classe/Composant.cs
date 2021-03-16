using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera
{
    public class Composant
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
            try
            {
                HttpResponseMessage response = await App.httpClient.GetAsync("http://localhost:5000/composant");
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseString = JsonConvert.DeserializeObject<Composant[]>(responseContent);
                return responseString;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des composants");
            }

        }

        public static async Task<Composant> CreateComposant(Composant composant)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("description",composant.description),
                    new KeyValuePair<string, string>("nomCaracteristique",composant.nomCaracteristique),
                    new KeyValuePair<string, string>("nomComposant",composant.nomComposant)
                };

                HttpResponseMessage response = await client.PostAsync(
                    "http://localhost:5000/gamme",
                    new FormUrlEncodedContent(values)
                );


                var composantResponse = await response.Content.ReadAsStringAsync();
                var composantJson = JsonConvert.DeserializeObject<Composant>(composantResponse);


                return composantJson;
            }
        }
    }
}
