using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using System.Net.Http.Json;
using System.Net.Http.Headers;


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
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODQxNDA1ODYsImxvZ2luIjoiYWRtaW4ifQ.xBeHNiIwspdEHhd-95TrXp-lIjyY5sefoboA6YZT0Xk");
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

        public async Task PostStation(string name, string ipadress, string macAddress)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODQxNDA1ODYsImxvZ2luIjoiYWRtaW4ifQ.xBeHNiIwspdEHhd-95TrXp-lIjyY5sefoboA6YZT0Xk");
                var station = new Station { StationName = name,IpAddress = ipadress, MacAddress = macAddress };
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

        public async Task RegisterStation()
        {
            ApiHandler api = new ApiHandler();
            Station station = new Station();
            Console.WriteLine("Registering the station");
            await api.PostStation(station.getStationName(), station.GetIPAddress(), station.GetMACAddress());
        }


    }
}

