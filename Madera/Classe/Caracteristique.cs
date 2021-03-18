using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    public class Caracteristique
    {
        public string _id { get; set; }
        public string nomCaracteristique { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public string description { get; set; }
        public override string ToString()
        {
            return this.nomCaracteristique;
        }

        public static async Task<Caracteristique[]> GetAllCaracteristique()
        {
            var response = App.httpClient.GetAsync("http://localhost:5000/caracteristique").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;
                var listCaracteristique = JsonConvert.DeserializeObject<Caracteristique[]>(responseString);
                return listCaracteristique;
            }
            return null;
        }
    }
}
