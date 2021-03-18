using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    public class Module
    {
        public string _id { get; set; }
        public Composant[] composants { get; set; }
        public string nomModule { get; set; }
        public string nomGamme { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string description { get; set; }
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
            var response = App.httpClient.GetAsync("http://localhost:5000/module/all").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;
                var listClient = JsonConvert.DeserializeObject<Module[]>(responseString);
                return listClient;
            }
            return null;
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

        public static async Task<Module> CreateModule(Module module)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("nomModule", module.nomModule),
                    new KeyValuePair<string, string>("description",module.description)
                };

                //var valuesComposants = new List<KeyValuePair<string, Composant[]>>
                //{
                 //   new KeyValuePair<string,Composant[]>("composants",module.composants)
                //};

                HttpResponseMessage response = await client.PostAsync(
                    "http://localhost:5000/module",
                    new FormUrlEncodedContent(values)
                );


                var appointmentResponse = await response.Content.ReadAsStringAsync();
                var appointmentJson = JsonConvert.DeserializeObject<Module>(appointmentResponse);


                return appointmentJson;
            }

        }
    }
}
