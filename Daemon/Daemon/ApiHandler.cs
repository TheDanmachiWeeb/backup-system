using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using System.Net.Http.Json;

namespace Daemon
{
    internal class ApiHandler
    {
        private string apiUrl = "http://localhost:5666/api";

        public async Task Connect()
        {
            // Replace with your API URL

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(apiUrl + "/users"); // Replace with your endpoint path
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                    }
                    else
                    {
                        Console.WriteLine($"Request failed with status code {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request failed: {ex.Message}");
                }

            }
        }

        public async Task RegisterStation(string name, string macAddress)
        {
            using (var httpClient = new HttpClient())
            {

                var station = new Station { name = name, macAdress = macAddress };
                var response = await httpClient.PostAsJsonAsync($"{apiUrl}/stations", station);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Station registered successfully");
                }
                else
                {
                    Console.WriteLine($"Failed to register station with status code {response.StatusCode}");
                }

            }
    }

        }
    }
}
