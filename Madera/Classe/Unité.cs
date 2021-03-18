using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    public class Unité
    {
        public string _id { get; set; }
        public string uniteMesure { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public string description { get; set; }
        public override string ToString()
        {
            return this.uniteMesure;
        }

        public static async Task<Unité[]> GetAllUnités()
        {
                var response = App.httpClient.GetAsync("http://localhost:5000/unite").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    var listClient = JsonConvert.DeserializeObject<Unité[]>(responseString);
                    return listClient;
                }
            return null;
        }
    }
}
