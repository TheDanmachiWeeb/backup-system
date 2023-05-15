using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Daemon
{
    public class FileManager
    {
        private string fileName = "database_secret.txt";
        private string programFolder = AppDomain.CurrentDomain.BaseDirectory + "/secret";
      

        public FileManager() { }

        public bool CheckIDFile() {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/secret");
            string filePath = Path.Combine(programFolder,fileName);
            Console.WriteLine(filePath);
            if (File.Exists(filePath))
            {
                Console.WriteLine("ID file exists");
                return true;
            }
            else
            {

                File.Create(filePath);
                Console.WriteLine(File.GetCreationTime(filePath).ToString());
                return false;
            }
        }

        public void SaveID(string id)
        {
            string filePath = Path.Combine(programFolder, fileName);
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(id);
            writer.Close();
        }

        public string getID()
        {
            string ID = string.Empty;
            string filePath = Path.Combine(programFolder, fileName);

            StreamReader reader = new StreamReader(filePath);
            ID = reader.ReadToEnd();
            Console.WriteLine("id is " + ID);
            reader.Close();
            return ID;

        }

        public void Rollback()
        {
            string filePath = Path.Combine(programFolder, fileName);
             File.Delete(filePath);
             
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
