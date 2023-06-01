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
using System.IO;
using System.Security.Cryptography;

namespace Daemon
{
    internal class ApiHandler
    {
        private static string token;
        private string apiUrl = "http://localhost:5666/api";
 

        public async Task GetToken()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"{apiUrl}/sessions");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        JObject obj = JObject.Parse(responseContent);
                        token = obj["token"].ToString();
                    }
                    else
                    {
                        Console.WriteLine($"Request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task<status> GetStatus(string id)
        {
            bool approved = false;
            
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Station station = new Station();
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.GetAsync($"{apiUrl}/stations/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        FileManager manager = new FileManager();
                        JObject jsonObject = JObject.Parse(responseContent);
                        string stationString = jsonObject.ToString();
                        station = JsonConvert.DeserializeObject<Station>(stationString);
                        if (station.status == status.approved)
                        {
                            Console.WriteLine("Station approved");
                        }
                        else if (station.status == status.waiting)
                        {
                            Console.WriteLine("Station is waiting for approval");
                        }
                        else if (station.status == status.rejected)
                        {
                            Console.WriteLine("Station not approved");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Request for approval failed with status code {response.StatusCode}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Checking approval failed: {ex.Message}");
                }
                    return station!.status;  
            }
        }

        public async Task<List<BackupConfiguration>> GetConfigsByID(string id)
        {
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
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\secret\\configJson"))
                    {
                        string json = manager.getConfigs(); //gets it from file
                        configs = CreateConfigs(json);
                    }
                }
                return configs;
            }
        }
        public async Task MarkConfigAsFinished(BackupConfiguration config, bool reserse = false)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpContent emptyContent = new StringContent(string.Empty);
                    var response = await httpClient.PutAsJsonAsync($"{apiUrl}/configurations/finished/{config.configId}", emptyContent );
                    string requestPayload = await response.RequestMessage.Content.ReadAsStringAsync();

                     //Print or log the JSON payload
                   Console.WriteLine(requestPayload);

                    Console.WriteLine($"{apiUrl}/configurations/finished/{config.configId}", emptyContent);

                    if (response.IsSuccessStatusCode)
                    {
                        if (!reserse)
                        {
                            Console.WriteLine("Config marked as finished");
                        }
                    }
                    else
                    {
                        if (!reserse)
                        {
                            Console.WriteLine($"Failed to mark config as finished: {response.StatusCode}");
                        }
                        else Console.WriteLine($"Failed to mark periodic config: {config.configId} as unfinished, it should still work tho");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!reserse)
                {
                    Console.WriteLine($"Failed to mark config as finished: {ex.Message}");
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
                try
                {
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
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task PostReport(BackupReport report, bool old = false)
        {
            using (var httpClient = new HttpClient())
            {
               
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);  
                Console.WriteLine("Sending report to the server...");
        
                try
                {
                    var response = await httpClient.PostAsJsonAsync($"{apiUrl}/Reports", report);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Report sent");
                        if (old)
                        {
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\secret\\oldReports");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to send report {response.StatusCode}");
                        Console.WriteLine("Saving report locally");
                        if (!old)
                        {
                            saveThisReport(report);
                        }
                    }
                } 
                catch (Exception ex) 
                {
                    Console.WriteLine($"Failed to send report {ex.Message}");
                    Console.WriteLine("Saving report locally");
                    if (!old)
                    {
                        saveThisReport(report);
                    }
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
        public void saveThisReport(BackupReport report)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\secret\\oldReports";
            FileManager manager = new FileManager();
            string reportToSave;

            reportToSave = $"{report.stationId.ToString()};{report.configId.ToString()};{report.reportTime.ToString()};{report.backupSize};{report.success};{report.errorMessage}\n";
            manager.saveReports(path, reportToSave);
        }


    }


}

