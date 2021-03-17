using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public override string ToString()
        {
            return this.first_name;
        }

        public static async Task<Client[]> GetAllClient()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:5000/client").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    var listClient = JsonConvert.DeserializeObject<Client[]>(responseString);
                    return listClient;
                }
            }
            return null;
        }
    }

}
