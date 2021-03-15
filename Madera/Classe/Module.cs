using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera
{
    public class Module
    {
        public string _id { get; set; }
        public Composant[] composants { get; set; }
        public string nomModule { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string composantsString
        {

            get => listComposantToString(composants);
        }

        public override string ToString()
        {
            return this.nomModule;
        }

        public static async Task<Module[]> GetAllModule()
        {
            try
            {
                HttpResponseMessage response = await App.httpClient.GetAsync("http://localhost:5000/module/all");
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseString = JsonConvert.DeserializeObject<Module[]>(responseContent);
                return responseString;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des modules");
            }
        }

        private string listComposantToString(Composant[] composants)
        {
            string result = "";
            foreach (var item in composants)
            {
                result += item.ToString() + ", ";
            }
            return result != "" ? result.Substring(0, result.Length - 2) : result;
        }
    }
}
