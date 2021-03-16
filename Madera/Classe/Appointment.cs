using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Madera.Classe
{
    public class Appointment
    {
        public string _id { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public string client { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public static async Task<Appointment[]> GetAllAppointment()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:5000/appointement").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    var listAppointment = JsonConvert.DeserializeObject<Appointment[]>(responseString);
                    return listAppointment;
                }
            }
            return null;
        }

        public static async Task<Appointment[]> GetAppointmentDay(DateTime date)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:5000/appointement/" + date.ToString("MM-dd-yyyy")).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    var listAppointment = JsonConvert.DeserializeObject<Appointment[]>(responseString);
                    return listAppointment;
                }
            }
            return null;
        }

        public static async Task<Appointment> PostAppointment(Appointment appointment)
        {
            try
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("name", appointment.name),
                    new KeyValuePair<string, string>("date",appointment.date.ToString("O", new CultureInfo("en-us"))),
                    new KeyValuePair<string, string>("client",appointment.client)
                };

                HttpResponseMessage response = await App.httpClient.PostAsync(
                    "http://localhost:5000/appointement",
                    new FormUrlEncodedContent(values)
                );


                var appointmentResponse = await response.Content.ReadAsStringAsync();
                var appointmentJson = JsonConvert.DeserializeObject<Appointment>(appointmentResponse);


                return appointmentJson;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }


        public static async Task<Appointment> DeleteAppointment(string id)
        {
            try
            {

                HttpResponseMessage response = await App.httpClient.DeleteAsync(
                    "http://localhost:5000/appointement/" + id
                );


                var appointmentResponse = await response.Content.ReadAsStringAsync();
                var appointmentJson = JsonConvert.DeserializeObject<Appointment>(appointmentResponse);


                return appointmentJson;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }
    }
}
