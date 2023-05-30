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
using static System.Net.Mime.MediaTypeNames;

namespace Daemon
{
    internal class ApiHandler
    {
        private string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODU2MjA3MTYsImxvZ2luIjoiZGltYSJ9.doWIqp4g-8aMLyHXkY2lXspZS9WS8jXIDgJh_9Tr4zI";
        private string apiUrl = "http://localhost:5666/api";
 


        public async Task<List<BackupConfiguration>> GetConfigsByID(string id)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\secret" + "\\" + "oldReports";
            FileManager manager = new FileManager();

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
                        manager.saveConfigs(responseContent);
                    }
                    else
                    {
                        Console.WriteLine($"Request failed with status code {response.StatusCode}");
                       
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\secret\\configJson"))
                        {
                            string json = manager.getConfigs(); //gets it from file
                            configs = CreateConfigs(json);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Getting configs failed: {ex.Message}");
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
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\secret\\oldReports";
                Console.WriteLine("Sending report to the server...");
                FileManager manager = new FileManager();

                var response = await httpClient.PostAsJsonAsync($"{apiUrl}/Reports", report);

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
               // input.Split("@@@", StringSplitOptions.None);

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
                FileManager manager = new FileManager();
                JObject jsonObject = JObject.Parse(jsonResponse);
                string config = jsonObject["configs"].ToString();
                configurations = JsonConvert.DeserializeObject<List<BackupConfiguration>>(config);
            }
            catch (Exception ex)
            {
                // write the exception
                Console.WriteLine($"error: {ex.Message}");
            }
            return configurations;
        }


    }


}

