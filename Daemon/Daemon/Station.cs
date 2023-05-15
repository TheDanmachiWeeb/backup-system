using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Daemon
{
    public class Station
    {

        public string StationName { get; set; }

        public string IpAddress { get; set; }

        public string MacAddress { get; set; }


        public string getStationName()
        {
            return Environment.MachineName;
        }

        public string GetIPAddress()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            // Find the first IP address
            IPAddress ipv4Address = null!;
            try
            {
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4Address = address;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            // Return the first IP found or return an empty string
            return ipv4Address?.ToString() ?? string.Empty;
        }

        public string GetMACAddress()
        {
            string macAddress = string.Empty;

            try
            {   
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        macAddress = nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return macAddress;
        }
    }

}
