using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera
{
    public class Client
    {
        public string _id { get; set; }
        public string first_name { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public static async Task<Client[]> GetAllClient()
        {
            HttpResponseMessage response = await App.httpClient.GetAsync($"http://localhost:5000/client");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsStringAsync();
                var aaa = JsonConvert.DeserializeObject<Client[]>(products);

                return aaa;
            }
            return null;
        }
    }

}
