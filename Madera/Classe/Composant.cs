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
    }
}
