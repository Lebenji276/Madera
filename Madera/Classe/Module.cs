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
        public String[] composants { get; set; }
        public Composant[] composantsArray { get; set; }
        public string nomModule { get; set; }
        public string nomGamme { get; set; } = "";
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string description { get; set; } = "";

        public string composant { get; set; }
        public string composantsString
        {

            get => listComposantToString(composantsArray);
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
                throw new Exception("Impossible de récupérer la liste des rendez-vous");
            }
        }

        private string listComposantToString(Composant[] composants)
        {
            string result = "";
            if (composants != null && composants.Length != 0)
            {
                foreach (var item in composants)
                {
                    result += item.ToString() + ", ";
                }
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
                    new KeyValuePair<string, string>("description",module.description),
                    new KeyValuePair<string,string>("composants",module.composant)
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
