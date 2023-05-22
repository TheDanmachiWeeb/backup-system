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
using static Daemon.ApiHandler;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Daemon
{
    internal class ApiHandler
    {
        private string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODQ5OTc5NDQsImxvZ2luIjoiYWRtaW4ifQ.xGAXWRT8hr15jTaieirEcunSerD82HLoeIpaEzwHlrM";
        private string apiUrl = "http://localhost:5666/api";



        public async Task<List<BackupConfiguration>> GetConfigsByID(string id)
        {
            using (var httpClient = new HttpClient())
            {
                List<BackupConfiguration> configs = new List<BackupConfiguration>();
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.GetAsync($"{apiUrl}/stations/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        configs = CreateConfigs(responseContent);
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
                return configs;
            }
        }

        public async Task<string> PostStation()
        {
            using (var httpClient = new HttpClient())
            {
                Console.WriteLine("Registering the station...");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
                    return string.Empty;
                }
            }
        }

        public async Task PostReport(BackupReport report)
        {
            using (var httpClient = new HttpClient())
            {
                Console.WriteLine("Sending report to the server...");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


               // Console.WriteLine(JsonConvert.SerializeObject(report));

                var response = await httpClient.PostAsJsonAsync($"{apiUrl}/Reports", report);
               // Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Report sent");
                }
                else
                {
                    Console.WriteLine($"Failed to send report {response.StatusCode}");
                }
            }
        }

            public async Task RegisterStation()
        {
            FileManager manager = new FileManager();
            ApiHandler api = new ApiHandler();
            Station station = new Station();
            string ID = await api.PostStation();

            if (ID != "")
            {
                Console.WriteLine("Saving response data...");
                manager.SaveID(ID.ToString());
                Console.WriteLine("ID saved");
            }
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
                    ID = match.Groups[1].Value;
                }
                else
                {
                throw new Exception("Response didn't contain ID");
                }

            return ID;
        }

        public List<BackupConfiguration> CreateConfigs(string jsonResponse)
        {
            List<BackupConfiguration> configurations = new List<BackupConfiguration>();
            try
            {
                JObject jsonObject = JObject.Parse(jsonResponse);
                string config = jsonObject["configs"].ToString();
                configurations = JsonConvert.DeserializeObject<List<BackupConfiguration>>(config);
            }
            catch (Exception ex)
            {
                // write the exception
                Console.WriteLine($"Serialization error: {ex.Message}");
            }
            return configurations;
        }


    }


}

