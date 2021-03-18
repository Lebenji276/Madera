using Madera.Classe;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera
{
    public class Gamme
    {
        public Composant[] composants { get; set; }
        public string nomGamme { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string _id { get; set; }

        public string module { get; set; }

        public string description { get; set; }
        public string listmodulesString { get; set; }
        public List<Module> listmodules { get; set; }
        public override string ToString()
        {
            return this.nomGamme;
        }

        public static async Task<Gamme[]> GetAllGammes()
        {
            var response = await App.httpClient.GetAsync("http://localhost:5000/gamme/all");

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var listGammes = JsonConvert.DeserializeObject<Gamme[]>(responseString);

                return listGammes;
            }
            return null;
        }

        public static async Task<Gamme> CreateGamme(Gamme gamme)
        {
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("nomGamme", gamme.nomGamme),
                new KeyValuePair<string, string>("description",gamme.description),
                new KeyValuePair<string,string>("modules",gamme.module)
            };

            HttpResponseMessage response = await App.httpClient.PostAsync(
                "http://localhost:5000/gamme/post",
                new FormUrlEncodedContent(values)
            );


            var appointmentResponse = await response.Content.ReadAsStringAsync();
            var appointmentJson = JsonConvert.DeserializeObject<Gamme>(appointmentResponse);


            return appointmentJson;
            
        }
        public static async Task<Gamme> UpdateGamme(Gamme gamme)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    //new KeyValuePair<string, string>("nomGamme", gamme.nomGamme),
                    //new KeyValuePair<string, string>("description",gamme.description),
                    new KeyValuePair<string,string>("modules",gamme.module)
                };

                HttpResponseMessage response = await client.PutAsync(
                    "http://localhost:5000/gamme/modules/"+ gamme._id,
                    new FormUrlEncodedContent(values)
                );


                var appointmentResponse = await response.Content.ReadAsStringAsync();
                var appointmentJson = JsonConvert.DeserializeObject<Gamme>(appointmentResponse);


                return appointmentJson;
            }

        }

    }
}
