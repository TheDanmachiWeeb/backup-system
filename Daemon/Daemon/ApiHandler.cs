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
        private string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODQyNTgzNTYsImxvZ2luIjoiYWRtaW4ifQ.vsJUsgJ1Q3ul8zpW2SZ-si1dBlDwV3CUNPF-ZAR1Br8";
        private string apiUrl = "http://localhost:5666/api";



        public async Task GetConfigsByID(string id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.GetAsync($"{apiUrl}/stations/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        CreateConfigs(responseContent);
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

            GetConfigsByID(ID);
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
            try
            {
                JObject jsonObject = JObject.Parse(jsonResponse);
                string config = jsonObject["configs"].ToString();
                List<BackupConfiguration> configurations = JsonConvert.DeserializeObject<List<BackupConfiguration>>(config);

                return configurations;
            }
            catch (Exception ex)
            {
                // write the exception
                Console.WriteLine($"Serialization error: {ex.Message}");
            }
            List<BackupConfiguration> backupConfigurations1 = new List<BackupConfiguration>();
            var backupConfiguration1 = new BackupConfiguration
            {
                configId = 4,
                BackupType = BackupType.Full,
                destinations = new List<destination>()
            };
            backupConfigurations1.Add(backupConfiguration1);
            return backupConfigurations1;
        }


    }


}

