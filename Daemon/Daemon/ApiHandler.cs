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
using System.Xml.Linq;
using System.Text.RegularExpressions;

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
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODQxNjU2MDYsImxvZ2luIjoiYWRtaW4ifQ.gUI5pau7qFN2i--x3R1vn-GjhJGGb-w0F0JMzUstPKE");
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

        public async Task<string> PostStation()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODQxNzM4MDksImxvZ2luIjoiYWRtaW4ifQ.eht_d-r1PB8etwbVK5Rn2-fqtuxr9ATmEyzFGikGxGc");

                var station = new Station();
                var response = await httpClient.PostAsJsonAsync($"{apiUrl}/stations", station);

                if (response.IsSuccessStatusCode)
                {


                    string responseContent = await response.Content.ReadAsStringAsync();
                    string stationId = String.Empty;

                    stationId = RegexID(responseContent);


                    Console.WriteLine("Station registered successfully. Station ID: " + stationId);
                    return stationId;

                }
                else
                {

                    Console.WriteLine($"Failed to register station with status code {response.StatusCode}");
                    FileManager manager = new FileManager();
                    manager.Rollback();
                    return string.Empty;

                }

            }
        }

        public async Task RegisterStation()
        {
            FileManager manager = new FileManager();
            ApiHandler api = new ApiHandler();
            Station station = new Station();
            Console.WriteLine("Registering the station...");
            string ID = await api.PostStation();
            Console.Clear();
            Console.WriteLine("Saving response data...");
            manager.SaveID(ID.ToString());
            Console.WriteLine("ID saved");
        }

        public string RegexID(string response)
        {
                string ID = string.Empty;
                string input = response;

                // Regex pattern to match the number after "stationId":
                string pattern = "\"stationId\":(\\d+)";

                Match match = Regex.Match(input, pattern);

                if (match.Success)
                {
                    // Extract the number captured in the first group.
                    ID = match.Groups[1].Value;
                }
                else
                {
                    Console.WriteLine("No ID given to the station...");
                }

            return ID;
        }


    }
}

