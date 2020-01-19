using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Eventmanager
{
    public class Save
    {
        public void CreateDirectory()
        {
            string autosavePath = Directory.GetCurrentDirectory() + "\\Save";

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(autosavePath))
                {
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(autosavePath);
            }
            catch (Exception e)
            {
                //
            }
        }

        public void Autosave(List<Teilnehmer> Starterliste)
        {
            string autosavePath = Directory.GetCurrentDirectory() + "\\Save";

            try
            {
                if (Directory.Exists(autosavePath))
                {
                    string path = Directory.GetCurrentDirectory() + "\\Save\\Autosave.json";
                    using (StreamWriter file = File.CreateText(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(file, Starterliste);
                    }

                }
            }
            catch (Exception e)
            {
                //
            }
        }

        public void SaveTurnier(string turniername, List<Teilnehmer> Starterliste)
        {

            // Configure save file dialog box
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = turniername; // Default file name
            dlg.DefaultExt = ".json"; // Default file extension
            dlg.Filter = "json files (.json)|*.json"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document

                using (StreamWriter file = File.CreateText(dlg.FileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, Starterliste);
                }
            }
        }
    }
}
