using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera
{
    public class ModuleCreatedevis
    {
        public string _id { get; set; }
        public string[] composants { get; set; }
        public string nomModule { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string description { get; set; }
        public override string ToString()
        {
            return this.nomModule;
        }

        public static async Task<ModuleCreatedevis[]> GetAllModuleCreatedevis()
        {
            try
            {
                HttpResponseMessage response = await App.httpClient.GetAsync("http://localhost:5000/module/all");
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseString = JsonConvert.DeserializeObject<ModuleCreatedevis[]>(responseContent);
                return responseString;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Impossible de récupérer la liste des modules");
            }
        }
    }
}
