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
        public Composant[] modules { get; set; }
        public string nomGamme { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string _id { get; set; }

        public string description { get; set; }
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
                Console.WriteLine("test");
                return listGammes;
            }
            return null;
        }

        public static async Task<Gamme> CreateGamme(Gamme gamme)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("nomGamme", gamme.nomGamme),
                    new KeyValuePair<string, string>("description",gamme.description)
                };

                HttpResponseMessage response = await client.PostAsync(
                    "http://localhost:5000/gamme",
                    new FormUrlEncodedContent(values)
                );


                var appointmentResponse = await response.Content.ReadAsStringAsync();
                var appointmentJson = JsonConvert.DeserializeObject<Gamme>(appointmentResponse);


                return appointmentJson;
            }
            
        }


    }
}
