using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class Setup
    {
        public async Task<List<BackupConfiguration>> SetupConfigs() {

            FileManager manager = new FileManager();
            ApiHandler api = new ApiHandler();
            List<BackupConfiguration> configurations = new List<BackupConfiguration>();
                await api.GetToken();
                bool file = manager.CheckIDFile();
                string IDString = manager.getID();

                if (!file || string.IsNullOrEmpty(IDString))
                {
                    await api.RegisterStation();
                    IDString = manager.getID(); // Update IDString after registration
                }

                Console.WriteLine("Your ID in the database: " + IDString);
                configurations = await api.GetConfigsByID(IDString);

            return configurations;
        }
    }
}
