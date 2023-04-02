using BackupSystem.Models;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Text;

namespace BackupSystem
{
    public class APIHandler
    {
        private HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:5666") };
        public async Task<Configuration> GetConfig(int configId)
        {
            Configuration? config = await client.GetFromJsonAsync<Configuration>($"/api/configurations/{configId}");
            
            return config!;
        }
    }
}
