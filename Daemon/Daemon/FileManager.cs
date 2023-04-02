using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daemon
{
    public class FileManager
    {
        public FileManager()
        {

        }
        public void SaveSnapshot(BackupConfiguration config, Snapshot snapshot)
        {
            string filename = "Snapshot_" + config.ID.ToString();
            string directoryname = "Snapshots";
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryname);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryname, filename);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(snapshot.Data[0] + ";" + snapshot.Data[1]);
            streamWriter.Close();
        }

        public Snapshot ReadSnapshot(BackupConfiguration config)
        {
            Snapshot snapshot;
            string filename = "Snapshot_" + config.ID.ToString();
            string directoryname = "Snapshots";

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryname, filename);

            StreamReader streamReader = new StreamReader(filePath);
            string line = streamReader.ReadLine();

            streamReader.Close();

            string[] data = line.Split(';');
            snapshot = new Snapshot(data);

            return snapshot;
        }
    }
}
